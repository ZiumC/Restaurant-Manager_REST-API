using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.CustomersService;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IClientApiService _clientApiService;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IConfiguration _config;

        public UsersController(IClientApiService clientApiService, IEmployeeApiService employeeApiService, IConfiguration config)
        {
            _clientApiService = clientApiService;
            _employeeApiService = employeeApiService;
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



            return Ok();
        }
    }
}
