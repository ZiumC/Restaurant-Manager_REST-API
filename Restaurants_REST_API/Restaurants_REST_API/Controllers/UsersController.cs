using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Models.DatabaseModel;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.UsersService;
using Restaurants_REST_API.Services.JwtService;
using Restaurants_REST_API.Utils.MapperService;
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
        private readonly string _loginRegex;
        private readonly string _emailRegex;
        private readonly string _peselRegex;
        private readonly string _baseCharactersSalt;

        public UsersController(IUserApiService userApiService, IEmployeeApiService employeeApiService, IConfiguration config, IJwtService jwtService)
        {
            _employeeApiService = employeeApiService;
            _userApiService = userApiService;
            _config = config;
            _jwtService = jwtService;

            _loginRegex = _config["ApplicationSettings:DataValidation:LoginRegex"];
            _emailRegex = _config["ApplicationSettings:DataValidation:EmailRegex"];
            _peselRegex = _config["ApplicationSettings:DataValidation:PeselRegex"];

            _baseCharactersSalt = _config["ApplicationSettings:Security:SaltBase"]

            try
            {
                _saltLength = int.Parse(_config["ApplicationSettings:Security:SaltLength"]);
                _maxLoginAttempts = int.Parse(_config["ApplicationSettings:MaxLoginAttempts"]);
                _amountBlockedDays = int.Parse(_config["ApplicationSettings:AmountBlockedDays"]);

                if (string.IsNullOrEmpty(_loginRegex))
                {
                    throw new Exception("Login regex can't be empty.");
                }

                if (string.IsNullOrEmpty(_emailRegex))
                {
                    throw new Exception("Email regex can't be empty.");
                }

                if (string.IsNullOrEmpty(_peselRegex))
                {
                    throw new Exception("Pesel regex can't be empty.");
                }

                if (_saltLength < 0 || _saltLength > 10 )
                {
                    throw new Exception("Length of password salt is invalid. Length should be between 1 and 10 characters.");
                }

                if (string.IsNullOrEmpty(_baseCharactersSalt))
                {
                    throw new Exception("Base characters for salt can't be empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Registers new user.
        /// </summary>
        /// <param name="newUser">Register data.</param>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterNewUser(PostUserDTO newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Data can't be null");
            }

            if (!Regex.Match(newUser.Login, _loginRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("Login is invalid");
            }

            if (!Regex.Match(newUser.Email, _emailRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("Email is invalid");
            }

            User? existingUser = await _userApiService.GetUserDataByEmail(newUser.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exist");
            }

            Employee? emp = null;
            if (newUser.RegisterMeAsEmployee)
            {
                string? pesel = newUser.PESEL;
                if (!string.IsNullOrEmpty(pesel))
                {
                    if (pesel.Count() != 11 || !Regex.Match(pesel, _peselRegex, RegexOptions.IgnoreCase).Success)
                    {
                        return BadRequest("PESEL is invalid");
                    }
                }

                if (newUser?.HiredDate == null)
                {
                    return BadRequest("Hired date is required");
                }

                Employee? basicExistingEmpData = await _employeeApiService.GetEmployeeSimpleDataByPeselAsync(pesel);
                if (basicExistingEmpData == null)
                {
                    return BadRequest("Given employee data are invalid");
                }

                if (newUser.HiredDate.Date != basicExistingEmpData.HiredDate.Date)
                {
                    return BadRequest("Given employee data are invalid");
                }

                //checking if employee is already registered
                User? userByEmpId = await _userApiService.GetUserDataByEmpId(basicExistingEmpData.IdEmployee);
                if (userByEmpId != null)
                {
                    /*
                     * this bad request message is same as well as above bad request messages 
                     * due to not allow brute force methods does pesel exist or not
                     */
                    return BadRequest("Given employee data are invalid");
                }
                emp = basicExistingEmpData;
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
                userToSave.IdEmployee = emp?.IdEmployee;
                userToSave.UserRole = new MapUserRolesUtility(_config)
                    .GetUserRoleBasedOnEmployeeTypesId(emp?.EmployeeInRestaurant.Select(eir => eir.IdType));

                bool isEmployeeRegistrationCompletedSuccess = await _userApiService.RegisterNewEmployeeAsync(userToSave);
                if (!isEmployeeRegistrationCompletedSuccess)
                {
                    return BadRequest("Something went wrong, unable to register as employee");
                }
                return Ok("Registration completed success");
            }
            else
            {
                userToSave.UserRole = new MapUserRolesUtility(_config).GetClientUserRole();

                bool isClientRegistrationCompletedSuccess = await _userApiService.RegisterNewClientAsync(userToSave);
                if (!isClientRegistrationCompletedSuccess)
                {
                    return BadRequest("Something went wrong, unable to register an client");
                }
                return Ok("Registration completed success");
            }
        }

        /// <summary>
        /// Allows to login user.
        /// </summary>
        /// <param name="loginRequest">Login data.</param>
        /// <returns>Pair of access and refresh token.</returns>
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
                 * this response message is specially returned even password isn't checked.
                 * This is because to prevent brute force attacks targeted to checking does
                 * login or password exist
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
                var accessToken = _jwtService.GenerateAccessTokenForUser(user);

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

        /// <summary>
        /// Renevs access token based on refresh token.
        /// </summary>
        /// <param name="jwt">Pair of refresh token and expired access token.</param>
        /// <returns>Pair of access and refresh token.</returns>
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
                 * this response message is specially returned. This is because 
                 * to prevent brute force attacks targeted to checking does
                 * refresh token exist.
                 */
                return Unauthorized("Refresh token is invalid");
            }

            var dateBlockedTo = userByRefreshToken.DateBlockedTo;
            if (dateBlockedTo != null)
            {
                if (dateBlockedTo > DateTime.Now)
                {
                    return Unauthorized($"You can't refresh token due to {dateBlockedTo}");
                }
            }

            string refreshToken = _jwtService.GenerateRefreshToken();
            string accessToken = _jwtService.GenerateAccessTokenForUser(userByRefreshToken);

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

            for (int i = 0; i < length; i++)
            {
                Random random = new Random();
                char c = _baseCharactersSalt[random.Next(_baseCharactersSalt.Length)];
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
