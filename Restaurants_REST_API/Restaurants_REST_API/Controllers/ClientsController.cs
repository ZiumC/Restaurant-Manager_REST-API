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
        /// Return full client data with all reservations.
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
        /// Adds new complaint to database for reservation based on reservation id and client id.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        /// <param name="newComplaint">Complaint data</param>
        /// <returns></returns>
        [HttpPost("{clientId}/reservation/{reservationId}/complain")]
        public async Task<IActionResult> MakeComplain(int clientId, int reservationId, PostComplaintDTO newComplaint)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            if (newComplaint == null || newComplaint.Message.Replace("\\s", "") == "")
            {
                return BadRequest("Complaint message can't be empty");
            }

            GetReservationDTO? reservationDetails = await _clientApiService.GetReservationDetailsByCliennIdReservationIdAsync(clientId, reservationId);
            if (reservationDetails == null)
            {
                return NotFound("Reservation associated with client not found");
            }

            if (reservationDetails.ReservationDate > DateTime.Now)
            {
                return BadRequest("Unable to make complain because reservation doesn't started yet");
            }

            string currentReservationStatus = reservationDetails.Status;
            string newStatus = _config["ApplicationSettings:ReservationStatus:New"];
            string canceledStatus = _config["ApplicationSettings:ReservationStatus:Canceled"];
            if (currentReservationStatus == newStatus || currentReservationStatus == canceledStatus)
            {
                return BadRequest("Unable to make complain because reservation is new or canceled");
            }

            if (reservationDetails.ReservationComplaint != null)
            {
                return BadRequest("Complaint has been already made");
            }

            var complaint = new GetComplaintDTO
            {
                Message = newComplaint.Message,
                Status = _config["ApplicationSettings:ComplaintStatus:New"],
                ComplaintDate = DateTime.Now
            };
            bool isComplaintMade = await _clientApiService.MakeComplainByClientIdAsync(clientId, reservationDetails, complaint);
            if (!isComplaintMade)
            {
                return BadRequest("Unable to make complaint");
            }

            return Ok("Complaint has been made");
        }

        /// <summary>
        /// Updates reservation data, this method confirms reservation.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        [HttpPut("{clientId}/reservation/{reservationId}/confirm")]
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
            string newStatus = _config["ApplicationSettings:ReservationStatus:New"];
            string confirmedStatus = _config["ApplicationSettings:ReservationStatus:Confirmed"];
            if (currentReservationStatus == newStatus)
            {
                if (reservationDetails.ReservationDate <= DateTime.Now)
                {
                    return BadRequest("Unable to confirm reservation because date of reservation is passed away");
                }

                reservationDetails.Status = confirmedStatus;

                bool isConfirmed = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isConfirmed)
                {
                    return BadRequest("Unable to confirm reservation");
                }
                return Ok("Reservation has been confirmed");
            }
            else if (currentReservationStatus == confirmedStatus)
            {
                return BadRequest("Reservation is already confirmed");
            }
            else
            {
                return BadRequest("Reservation can't be confirmed because is canceled or rated");
            }

        }

        /// <summary>
        /// Updates reservation data, this method cancels reservation.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        /*
         * This is duplication of method ConfirmReservation, 
         * because in future could be different logic implemented
        */
        [HttpPut("{clientId}/reservation/{reservationId}/cancel")]
        public async Task<IActionResult> CancelReseration(int clientId, int reservationId)
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
            string newStatus = _config["ApplicationSettings:ReservationStatus:New"];
            string confirmedStatus = _config["ApplicationSettings:ReservationStatus:Confirmed"];
            string canceledStatus = _config["ApplicationSettings:ReservationStatus:Canceled"];
            if (currentReservationStatus == newStatus || currentReservationStatus == confirmedStatus)
            {
                if (reservationDetails.ReservationDate <= DateTime.Now)
                {
                    return BadRequest("Unable to cancel reservation because date of reservation is passed away");
                }

                reservationDetails.Status = _config["ApplicationSettings:ReservationStatus:Canceled"];

                bool isConfirmed = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isConfirmed)
                {
                    return BadRequest("Unable to cancel reservation");
                }
                return Ok("Reservation has been canceled");
            }
            else if (currentReservationStatus == canceledStatus)
            {
                return BadRequest("Reservation is already canceled");
            }
            else
            {
                return BadRequest("Reservation can't be canceled because is rated");
            }
        }

        [HttpPut("{clientId}/reservation/{reservationId}/rate")]
        public async Task<IActionResult> RateReseration(int clientId, int reservationId, int grade)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            if (grade < 0 || grade > 10)
            {
                return BadRequest($"Reservation grade ({grade}) is invalid");
            }


            GetReservationDTO? reservationDetails = await _clientApiService.GetReservationDetailsByCliennIdReservationIdAsync(clientId, reservationId);
            if (reservationDetails == null)
            {
                return NotFound("Reservation associated with client not found");
            }

            if (reservationDetails.ReservationDate > DateTime.Now)
            {
                return BadRequest("Unable to rate reservation because reservation doesn't started yet");
            }

            string currentReservationStatus = reservationDetails.Status;
            string confirmedStatus = _config["ApplicationSettings:ReservationStatus:Confirmed"];
            string ratedStatus = _config["ApplicationSettings:ReservationStatus:Rated"];
            if (currentReservationStatus == confirmedStatus)
            {
                reservationDetails.ReservationGrade = grade;
                reservationDetails.Status = ratedStatus;

                bool isUpdated = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isUpdated) 
                {
                    return BadRequest("Unable to rate reservation");
                }
                return Ok("Reservation has been rated");
            }
            else if (currentReservationStatus == ratedStatus) 
            {
                return BadRequest("Reservation is rated already");
            }
            else
            {
                return BadRequest("Unable to rate reservation because is new or canceled");
            }
        }
    }
}
