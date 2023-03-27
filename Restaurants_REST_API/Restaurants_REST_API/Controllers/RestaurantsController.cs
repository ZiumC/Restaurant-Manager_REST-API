using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Services.Database_Service;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;

        public RestaurantsController(IRestaurantApiService restaurantsApiService)
        {
            _restaurantsApiService = restaurantsApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _restaurantsApiService.GetAllRestaurantsAsync();

            if (restaurants == null || restaurants.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }

            return Ok(restaurants);
        }


    }
}
