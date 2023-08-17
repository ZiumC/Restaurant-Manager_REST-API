using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.CustomersService;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    /*
     * This controller is for customers only! 
     * Business logic for customer and owner is completely 
     * different so this is why this controller may have 
     * similar endpoints to others existing controllers
     */
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientApiService _clientApiService;
        private readonly IRestaurantApiService _restaurantApiService;
        private readonly IConfiguration _config;


        public ClientsController(IClientApiService clientApiService, IRestaurantApiService restaurantApiService, IConfiguration config)
        {
            _clientApiService = clientApiService;
            _restaurantApiService = restaurantApiService;
            _config = config;
        }

        /// <summary>
        /// Return full client data with all reservations
        /// </summary>
        /// <param name="clientId">Client id</param>
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientData(int clientId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            GetClientDataDTO? clientData = await _clientApiService.GetClientDataByIdAsync(clientId);

            if (clientData == null)
            {
                return NotFound("Client not found");
            }

            IEnumerable<GetReservationDTO>? reservations = await _clientApiService.GetAllReservationsDataByClientIdAsync(clientId);

            if (reservations != null && reservations.Count() > 0)
            {
                clientData.ClientReservations = reservations.ToList();
            }

            return Ok(clientData);
        }

        /// <summary>
        /// Returns details of exactly one reservation that client has.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        [HttpGet("{clientId}/reservation/{reservationId}")]
        public async Task<IActionResult> GetClientReservationData(int clientId, int reservationId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            GetClientDataDTO? clientData = await _clientApiService.GetClientDataByIdAsync(clientId);

            if (clientData == null)
            {
                return NotFound("Client not found");
            }

            GetReservationDTO? reservationDetails = await _clientApiService.GetReservationDetailsByCliennIdReservationIdAsync(clientId, reservationId);

            if (reservationDetails == null)
            {
                return NotFound("Reservation not found");
            }

            return Ok(reservationDetails);
        }

        /// <summary>
        /// Adds new reservation to database, makes connections between relations.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="newReservation">Reservation data</param>
        [HttpPost("{clientId}/reservation")]
        public async Task<IActionResult> MakeReservation(int clientId, PostReservationDTO newReservation)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (newReservation == null)
            {
                return BadRequest("Reservation details are invalid");
            }

            if (!GeneralValidator.isNumberGtZero(newReservation.IdRestaurant))
            {
                return BadRequest($"Restaurant id={newReservation.IdRestaurant} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(newReservation.HowManyPeoples))
            {
                return BadRequest("Number of reservation peoples is invalid");
            }

            var clientData = await _clientApiService.GetClientDataByIdAsync(clientId);
            if (clientData == null)
            {
                return NotFound("Client not found");
            }

            var restaurantBasicData = await _restaurantApiService.GetBasicRestaurantDataByIdAsync(newReservation.IdRestaurant);
            if (restaurantBasicData == null)
            {
                return NotFound("Restaurant not found");
            }

            if (newReservation.ReservationDate < DateTime.Now)
            {
                return BadRequest("Reservation date can't be older than now");
            }

            bool isReservationMade = await _clientApiService.MakeReservationByClientIdAsync(clientId, newReservation);

            if (!isReservationMade)
            {
                return BadRequest("Unable to make reservation in that restaurant");
            }

            return Ok("Reservation has been made");
        }

        /// <summary>
        /// Updates reservation data, this method confirms reservation
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        [HttpPut("{clientId}/reservation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int clientId, int reservationId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            GetReservationDTO? reservationDetails = await _clientApiService.GetReservationDetailsByCliennIdReservationIdAsync(clientId, reservationId);
            if (reservationDetails == null)
            {
                return NotFound("Reservation associated with client not found");
            }

            string currentReservationStatus = reservationDetails.Status;
            if (currentReservationStatus == _config["ApplicationSettings:ReservationStatus:New"])
            {
                reservationDetails.Status = _config["ApplicationSettings:ReservationStatus:Confirmed"];

                bool isConfirmed = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isConfirmed) 
                {
                    return BadRequest("Unable to confirm reservation");
                }
                return Ok("Reservation has been confirmed");
            }
            else if (currentReservationStatus == _config["ApplicationSettings:ReservationStatus:Confirmed"])
            {
                return BadRequest("Reservation is already confirmed");
            }

            return BadRequest("Reservation can't be confirmed because is canceled or finished");
        }

    }
}
