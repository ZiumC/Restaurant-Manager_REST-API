using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IReservationApiService _reservationsApiService;

        public ReservationsController(IRestaurantApiService restaurantsApiService, IReservationApiService reservationsApiService)
        {
            _restaurantsApiService = restaurantsApiService;
            _reservationsApiService = reservationsApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            IEnumerable<GetReservationDTO>? reservations = await _reservationsApiService.GetAllReservationsAsync();

            if (reservations == null)
            {
                return NotFound($"Reservations not found");
            }

            return Ok(reservations);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetReservationBy(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            GetReservationDTO? reservation = await _reservationsApiService.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                return NotFound($"Reservation {id} not found");
            }

            return Ok(reservation);
        }

        [HttpGet]
        [Route("by-restaurant/id")]
        public async Task<IActionResult> GetReservationsByRestaurant(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Restaurant id={id} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            IEnumerable<GetReservationDTO>? reservations = await _reservationsApiService.GetReservationsByRestaurantIdAsync(id);

            if (reservations == null || reservations.Count() == 0)
            {
                return NotFound("Reservations not found");
            }

            return Ok(reservations);
        }

        [HttpGet]
        [Route("by-client/id")]
        public async Task<IActionResult> GetReservationsByClient(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Client id={id} is invalid");
            }

            GetClientDTO? client = await _reservationsApiService.GetReservationsByClientIdAsync(id);

            if (client == null)
            {
                return NotFound($"Client not found");
            }

            return Ok(client);
        }

    }
}
