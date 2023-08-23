using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.DatabaseModel;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.UsersService;
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
        private readonly int _saltLength;

        public UsersController(IUserApiService userApiService, IEmployeeApiService employeeApiService, IConfiguration config)
        {
            _employeeApiService = employeeApiService;
            _userApiService = userApiService;
            _config = config;
            try
            {
                _saltLength = int.Parse(_config["ApplicationSettings:Security:SaltLength"]);
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

            var existingUser = await _userApiService.GetUserDataBy(newUser.Email);
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
                LoginAttemps = 0,
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

            return Ok();   
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
