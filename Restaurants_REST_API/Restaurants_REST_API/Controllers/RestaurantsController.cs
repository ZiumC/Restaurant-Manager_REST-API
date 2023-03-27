using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;
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

            if (restaurants.Count() == 0 || restaurants == null)
            {
                return NotFound("Restaurants not found");
            }

            return Ok(restaurants);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetRestaurantBy(int id)
        {
            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantInfoByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            RestaurantDTO restaurantDTO = await _restaurantsApiService.GetRestaurantDetailsByIdAsync(id);

            return Ok(restaurantDTO);
        }

        [HttpGet]
        [Route("/reservations")]
        public async Task<IActionResult> GetReservations()
        {
            var reservations = await _restaurantsApiService.GetAllReservationsAsync();

            if (reservations == null) {
                return NotFound($"Reservations not found");
            }

            return Ok(reservations);
        }


    }
}
