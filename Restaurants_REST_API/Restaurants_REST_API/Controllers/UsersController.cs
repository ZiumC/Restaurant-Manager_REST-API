using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.DatabaseModel;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.UsersService;
using Restaurants_REST_API.Services.JwtService;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserApiService _userApiService;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;
        private readonly int _saltLength;
        private readonly int _maxLoginAttempts;
        private readonly int _amountBlockedDays;

        public UsersController(IUserApiService userApiService, IEmployeeApiService employeeApiService, IConfiguration config, IJwtService jwtService)
        {
            _employeeApiService = employeeApiService;
            _userApiService = userApiService;
            _config = config;
            _jwtService = jwtService;
            try
            {
                _saltLength = int.Parse(_config["ApplicationSettings:Security:SaltLength"]);
                _maxLoginAttempts = int.Parse(_config["ApplicationSettings:MaxLoginAttempts"]);
                _amountBlockedDays = int.Parse(_config["ApplicationSettings:AmountBlockedDays"]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser(PostUserDTO newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Data can't be null");
            }

            string loginRegex = _config["ApplicationSettings:DataValidation:LoginRegex"];
            string emailRegex = _config["ApplicationSettings:DataValidation:EmailRegex"];

            if (!Regex.Match(newUser.Login, loginRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("Login is invalid");
            }

            if (!Regex.Match(newUser.Email, emailRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("Email is invalid");
            }

            var existingUser = await _userApiService.GetUserDataByEmail(newUser.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exist");
            }

            if (newUser.RegisterMeAsEmployee)
            {
                string? pesel = newUser.PESEL;
                if (!string.IsNullOrEmpty(pesel))
                {
                    string peselRegex = _config["ApplicationSettings:DataValidation:PeselRegex"];
                    if (pesel.Count() != 11 || !Regex.Match(pesel, peselRegex, RegexOptions.IgnoreCase).Success)
                    {
                        return BadRequest("PESEL is invalid");
                    }
                }

                if (newUser?.HiredDate == null)
                {
                    return BadRequest("Hired date is required");
                }

                var basicExistingEmpData = await _employeeApiService.GetEmployeeDataByPeselAsync(pesel);
                if (basicExistingEmpData == null)
                {
                    return BadRequest("Given employee data are invalid");
                }

                if (newUser.HiredDate.Date != basicExistingEmpData.HiredDate.Date)
                {
                    return BadRequest("Given employee data are invalid");
                }

                var userByEmpId = await _userApiService.GetUserDataByEmpId(basicExistingEmpData.IdEmployee);
                if (userByEmpId != null)
                {
                    /*
                     * this bad request message is same as well as above bad request messages due to not
                     * give an someone to brute force and guess if pesel is correct or not
                     */
                    return BadRequest("Given employee data are invalid");
                }
            }

            string salt = GetSalt(_saltLength);
            string hashedPassword = GetHashedPasswordWithSalt(newUser.Password, salt);

            User userToSave = new User
            {
                Login = newUser.Login,
                Email = newUser.Email,
                Password = hashedPassword,
                PasswordSalt = salt,
                LoginAttempts = 0,
                DateBlockedTo = null
            };

            if (newUser.RegisterMeAsEmployee)
            {
                bool isEmployeeRegistrationCompletedSuccess = await _userApiService.RegisterNewEmployeeAsync(userToSave);
                if (!isEmployeeRegistrationCompletedSuccess)
                {
                    return BadRequest("Something went wrong, unable to register an employee");
                }
                return Ok("Registration completed success");
            }
            else
            {
                bool isClientRegistrationCompletedSuccess = await _userApiService.RegisterNewClientAsync(userToSave);
                if (!isClientRegistrationCompletedSuccess)
                {
                    return BadRequest("Something went wrong, unable to register an client");
                }
                return Ok("Registration completed success");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(PostLoginRequestDTO loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login request");
            }

            var user = await _userApiService.GetUserDataByLoginOrEmail(loginRequest.LoginOrEmail);
            if (user == null)
            {
                /*
                 * this response message is specially returned even password 
                 * isn't checking here because to not allow brut force methods 
                 * to check if login or email exist in db 
                 */
                return Unauthorized("Login or password are incorrect");
            }

            var dateBlockedTo = user.DateBlockedTo;
            if (dateBlockedTo != null)
            {
                if (dateBlockedTo > DateTime.Now)
                {
                    return Unauthorized($"You can't login due to {dateBlockedTo}");
                }
                else if (dateBlockedTo < DateTime.Now && user.LoginAttempts >= _maxLoginAttempts)
                {
                    user.LoginAttempts = 0;
                    await _userApiService.UpdateUserData(user);
                }
            }

            string hashedPassedPassword = GetHashedPasswordWithSalt(loginRequest.Password, user.PasswordSalt);
            if (hashedPassedPassword.Equals(user.Password))
            {
                user.LoginAttempts = 0;
                user.DateBlockedTo = null;

                var refreshToken = _jwtService.GenerateRefreshToken();
                var accessToken = _jwtService.GenerateAccessTokenForUserLogin(user.Login, "User");

                user.RefreshToken = refreshToken;
                bool isUpdated = await _userApiService.UpdateUserData(user);
                if (!isUpdated)
                {
                    return StatusCode(500, "Server side error, unable to give you an access token");
                }

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }
            else
            {
                user.LoginAttempts += 1;
                await _userApiService.UpdateUserData(user);

                if (user.LoginAttempts >= _maxLoginAttempts)
                {
                    user.DateBlockedTo = DateTime.Now.AddDays(_amountBlockedDays);
                    user.RefreshToken = null;
                    await _userApiService.UpdateUserData(user);
                    return Unauthorized($"Login or password are incorrect, you have been blocked to {user.DateBlockedTo}");
                }

                return Unauthorized($"Login or password are incorrect, you have {_maxLoginAttempts - user.LoginAttempts} attempts left");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(PostJwtDTO jwt) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid refresh request");
            }

            bool areTokensValid = _jwtService.ValidateJwt(jwt);
            if (!areTokensValid)
            {
                return Unauthorized("Tokens aren't valid to this server");
            }

            var userByRefreshToken = await _userApiService.GetUserDataByRefreshToken(jwt.RefreshToken);
            if (userByRefreshToken == null || string.IsNullOrEmpty(userByRefreshToken.RefreshToken))
            {
                /*
                 * this response message is specially returned to not 
                 * allow brute force methods to check if refresh token 
                 * exist in db
                 */
                return Unauthorized("Refresh token is invalid");
            }

            var dateBlockedTo = userByRefreshToken.DateBlockedTo;
            if (dateBlockedTo != null)
            {
                if (dateBlockedTo > DateTime.Now)
                {
                    return Unauthorized($"You can't login due to {dateBlockedTo}");
                }
            }

            string refreshToken = _jwtService.GenerateRefreshToken();
            string accessToken = _jwtService.GenerateAccessTokenForUserLogin(userByRefreshToken.Login, "User");

            userByRefreshToken.RefreshToken = refreshToken;
            userByRefreshToken.LoginAttempts = 0;
            userByRefreshToken.DateBlockedTo = null;

            bool isUpdated = await _userApiService.UpdateUserData(userByRefreshToken);
            if (!isUpdated)
            {
                return StatusCode(500, "Server side error, unable to give you an access token");
            }

            return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
        }


        private string GetSalt(int length)
        {
            if (length < 0 || length > 10)
            {
                throw new Exception("Salt length should be in range 1 - 10");
            }

            string result = "";
            string baseCharactersForSalt = _config["ApplicationSettings:Security:SaltBase"];

            for (int i = 0; i < length; i++)
            {
                Random random = new Random();
                char c = baseCharactersForSalt[random.Next(baseCharactersForSalt.Length)];
                result = result + c;
            }

            return result;
        }

        private string GetHashedPasswordWithSalt(string password, string salt)
        {
            var sha256algorithm = SHA256.Create();
            var hash = new StringBuilder();
            byte[] crypto = sha256algorithm.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
