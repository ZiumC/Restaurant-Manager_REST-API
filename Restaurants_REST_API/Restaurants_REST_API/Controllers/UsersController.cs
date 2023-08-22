using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Models.DatabaseModel;
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
        private readonly IConfiguration _config;
        private readonly int _saltLength = 10;

        public UsersController(IUserApiService userApiService, IConfiguration config)
        {
            _userApiService = userApiService;
            _config = config;
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

            string? pesel = newUser.PESEL;
            if (!string.IsNullOrEmpty(pesel))
            {
                string peselRegex = _config["ApplicationSettings:DataValidation:PeselRegex"];
                if (pesel.Count() != 11 || !Regex.Match(pesel, peselRegex, RegexOptions.IgnoreCase).Success)
                {
                    return BadRequest("PESEL is invalid");
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

            if (!string.IsNullOrEmpty(pesel))
            {
                bool isEmployeeRegistrationCompletedSuccess = await _userApiService.RegisterNewEmployeeAsync(userToSave);
                if (!isEmployeeRegistrationCompletedSuccess)
                {
                    return BadRequest("Something went wrong, unable to register a new user");
                }
                return Ok("Registration completed success");
            }
            else 
            {
                bool isClientRegistrationCompletedSuccess = await _userApiService.RegisterNewClientAsync(userToSave);
                if (!isClientRegistrationCompletedSuccess)
                {
                    return BadRequest("Something went wrong, unable to register a new user");
                }
                return Ok("Registration completed success");
            }
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
