using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
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

        public ClientsController(IClientApiService clientApiService) 
        {
            _clientApiService = clientApiService;
        }

        /// <summary>
        /// Return full client data with all reservations
        /// </summary>
        /// <param name="clientId">Client id</param>
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientData(int clientId) 
        {
            if (!GeneralValidator.isCorrectId(clientId))
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
            if (!GeneralValidator.isCorrectId(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(reservationId))
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



    }
}
