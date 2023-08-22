using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.CustomersService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IClientApiService _clientApiService;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IConfiguration _config;

        UsersController(IClientApiService clientApiService, IEmployeeApiService employeeApiService, IConfiguration config)
        {
            _clientApiService = clientApiService;
            _employeeApiService = employeeApiService;
            _config = config;
        }

    }
}
