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

        /// <summary>
        /// Returns all reservations data
        /// </summary>
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

        /// <summary>
        /// Returns reservation details by reservation id
        /// </summary>
        /// <param name="reservationId">Reservation id</param>
        [HttpGet("{reservationId}")]
        public async Task<IActionResult> GetReservationBy(int reservationId)
        {
            if (!GeneralValidator.isCorrectId(reservationId))
            {
                return BadRequest($"reservation id={reservationId} is invalid");
            }

            GetReservationDTO? reservation = await _reservationsApiService.GetReservationByIdAsync(reservationId);

            if (reservation == null)
            {
                return NotFound($"Reservation {reservationId} not found");
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Returns reservations details from specified restaurant id
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetReservationsByRestaurant(int restaurantId)
        {
            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            IEnumerable<GetReservationDTO>? reservations = await _reservationsApiService.GetRestaurantReservationsAsync(restaurantId);
            if (reservations == null || reservations.Count() == 0)
            {
                return NotFound("Reservations not found");
            }

            return Ok(reservations);
        }

        /// <summary>
        /// Returns reservations details by client id
        /// </summary>
        /// <param name="clientId">Client id</param>
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetReservationsByClient(int clientId)
        {
            if (!GeneralValidator.isCorrectId(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            GetClientDTO? client = await _reservationsApiService.GetReservationsByClientIdAsync(clientId);

            if (client == null)
            {
                return NotFound($"Client not found");
            }

            return Ok(client);
        }

    }
}
