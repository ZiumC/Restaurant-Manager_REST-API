using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IComplaintApiService _reservationsApiService;

        public ComplaintsController(IRestaurantApiService restaurantsApiService, IComplaintApiService reservationsApiService)
        {
            _restaurantsApiService = restaurantsApiService;
            _reservationsApiService = reservationsApiService;
        }

    }
}
