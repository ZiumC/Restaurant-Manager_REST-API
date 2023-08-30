using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Services;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.CustomersService;
using Restaurants_REST_API.Services.JwtService;
using Restaurants_REST_API.Services.ValidatorService;
using System.Security.Claims;

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
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _config;
        private readonly string _newReservationStatus;
        private readonly string _canceledReservationStatus;
        private readonly string _confirmedReservationStatus;
        private readonly string _ratedReservationStatus;
        private readonly string _newComplaintStatus;


        public ClientsController(IClientApiService clientApiService, IRestaurantApiService restaurantApiService, IJwtService jwtService, IConfiguration config)
        {
            _clientApiService = clientApiService;
            _restaurantApiService = restaurantApiService;
            _jwtService = jwtService;   
            _config = config;

            _newReservationStatus = _config["ApplicationSettings:ReservationStatus:New"];
            _canceledReservationStatus = _config["ApplicationSettings:ReservationStatus:Canceled"];
            _confirmedReservationStatus = _config["ApplicationSettings:ReservationStatus:Confirmed"];
            _ratedReservationStatus = _config["ApplicationSettings:ReservationStatus:Rated"];

            _newComplaintStatus = _config["ApplicationSettings:ComplaintStatus:New"];

            try 
            {
                if (string.IsNullOrEmpty(_newReservationStatus))
                {
                    throw new Exception("Reservation status (NEW) can't be empty");
                }

                if (string.IsNullOrEmpty(_canceledReservationStatus))
                {
                    throw new Exception("Reservation status (CANCELED) can't be empty");
                }

                if (string.IsNullOrEmpty(_confirmedReservationStatus))
                {
                    throw new Exception("Reservation status (CONFIRMED) can't be empty");
                }

                if (string.IsNullOrEmpty(_ratedReservationStatus))
                {
                    throw new Exception("Reservation status (RATED) can't be empty");
                }

                if (string.IsNullOrEmpty(_newComplaintStatus))
                {
                    throw new Exception("Complaint status (NEW) can't be empty");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Returns restaurants data with grade.
        /// </summary>
        /*
         * This endpoint for restaurant data has been added
         * here because this is visible for everyone.
         */
        [HttpGet("restaurants")]
        public async Task<IActionResult> GetRestaurantsData()
        {
            var allRestaurantsDetails = await _restaurantApiService.GetAllRestaurantsAsync();

            if (allRestaurantsDetails == null || allRestaurantsDetails.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }

            try
            {
                var result = allRestaurantsDetails
                .Select(ard => new
                {
                    IdRestaurant = ard.IdRestaurant,
                    Name = ard.Name,
                    Address = new
                    {
                        City = ard.Address.City,
                        Street = ard.Address.Street,
                        BuildingNumber = ard.Address.BuildingNumber,
                        LocalNumber = ard.Address.LocalNumber
                    },
                    MenuCount = ard.RestaurantDishes?.Count(),
                    Grade = ard.RestaurantReservations?
                    .Where(g => g != null)
                    .Average(g => g.ReservationGrade)
                })
                .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Something went wrong, unable to get restaurants data");
            }
        }

        /// <summary>
        /// Returns restaurant details with grade and aviable menu.
        /// </summary>
        /// <param name="retaurantId">Restaurant id</param>
        /*
         * This endpoint for restaurant details has been added
         * here because this is visible for everyone.
         */
        [HttpGet("restaurants/{retaurantId}")]
        public async Task<IActionResult> GetRestaurantDetailsBy(int retaurantId)
        {
            if (!GeneralValidator.isNumberGtZero(retaurantId))
            {
                return BadRequest($"Reestaurant id={retaurantId} is invalid");
            }

            var restaurantBasicInfo = await _restaurantApiService.GetBasicRestaurantDataByIdAsync(retaurantId);
            if (restaurantBasicInfo == null)
            {
                return NotFound("Restaurant not found");
            }

            var restaurantDetails = await _restaurantApiService.GetDetailedRestaurantDataAsync(restaurantBasicInfo);

            try
            {
                var result = new
                {
                    IdRestaurant = restaurantDetails.IdRestaurant,
                    Name = restaurantDetails.Name,
                    Address = new
                    {
                        City = restaurantDetails.Address.City,
                        Street = restaurantDetails.Address.Street,
                        BuildingNumber = restaurantDetails.Address.BuildingNumber,
                        LocalNumber = restaurantDetails.Address.LocalNumber
                    },
                    Menu = restaurantDetails.RestaurantDishes?
                    .Select(rd => new
                    {
                        Name = rd.Name,
                        Price = rd.Price
                    })
                    .ToList(),
                    Grade = restaurantDetails.RestaurantReservations?
                    .Where(g => g != null)
                    .Average(g => g.ReservationGrade)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Something went wrong, unable to get restaurant data");
            }
        }

        /// <summary>
        /// Return full client data with all reservations.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        [HttpGet("{clientId}")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> GetClientDataBy(int clientId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
            }

            GetClientDataDTO? clientData = await _clientApiService.GetClientDetailsByIdAsync(clientId);

            if (clientData == null)
            {
                return NotFound("Client not found");
            }

            IEnumerable<GetReservationDTO>? reservations = await _clientApiService.GetAllReservationsDetailsByClientIdAsync(clientId);

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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        [HttpGet("{clientId}/reservation/{reservationId}")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> GetClientReservationDataBy(int clientId, int reservationId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
            }

            GetClientDataDTO? clientData = await _clientApiService.GetClientDetailsByIdAsync(clientId);

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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        [HttpPost("{clientId}/reservation")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> MakeReservationBy(int clientId, PostReservationDTO newReservation)
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

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
            }

            GetClientDataDTO? clientData = await _clientApiService.GetClientDetailsByIdAsync(clientId);
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        [HttpPost("{clientId}/reservation/{reservationId}/complaint")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> MakeComplaintBy(int clientId, int reservationId, PostComplaintDTO newComplaint)
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

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
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
            if (currentReservationStatus == _newReservationStatus || currentReservationStatus == _canceledReservationStatus)
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
                Status = _newComplaintStatus,
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
        /// Updates reservation data, this endpoint confirms reservation.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        [HttpPut("{clientId}/reservation/{reservationId}/confirm")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> ConfirmReservationBy(int clientId, int reservationId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
            }

            GetReservationDTO? reservationDetails = await _clientApiService.GetReservationDetailsByCliennIdReservationIdAsync(clientId, reservationId);
            if (reservationDetails == null)
            {
                return NotFound("Reservation associated with client not found");
            }

            string currentReservationStatus = reservationDetails.Status;
            if (currentReservationStatus == _newReservationStatus)
            {
                if (reservationDetails.ReservationDate <= DateTime.Now)
                {
                    return BadRequest("Unable to confirm reservation because date of reservation is passed away");
                }

                reservationDetails.Status = _confirmedReservationStatus;

                bool isConfirmed = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isConfirmed)
                {
                    return BadRequest("Unable to confirm reservation");
                }
                return Ok("Reservation has been confirmed");
            }
            else if (currentReservationStatus == _confirmedReservationStatus)
            {
                return BadRequest("Reservation is already confirmed");
            }
            else
            {
                return BadRequest("Reservation can't be confirmed because is canceled or rated");
            }

        }

        /// <summary>
        /// Updates reservation data, this endpoint cancels reservation.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        /*
         * This is duplication of method ConfirmReservation, 
         * because in future could be different logic implemented
        */
        [HttpPut("{clientId}/reservation/{reservationId}/cancel")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> CancelReserationBy(int clientId, int reservationId)
        {
            if (!GeneralValidator.isNumberGtZero(clientId))
            {
                return BadRequest($"Client id={clientId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(reservationId))
            {
                return BadRequest($"Reservation id={reservationId} is invalid");
            }

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
            }

            GetReservationDTO? reservationDetails = await _clientApiService.GetReservationDetailsByCliennIdReservationIdAsync(clientId, reservationId);
            if (reservationDetails == null)
            {
                return NotFound("Reservation associated with client not found");
            }

            string currentReservationStatus = reservationDetails.Status;
            if (currentReservationStatus == _newReservationStatus || currentReservationStatus == _confirmedReservationStatus)
            {
                if (reservationDetails.ReservationDate <= DateTime.Now)
                {
                    return BadRequest("Unable to cancel reservation because date of reservation is passed away");
                }

                reservationDetails.Status = _canceledReservationStatus;

                bool isConfirmed = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isConfirmed)
                {
                    return BadRequest("Unable to cancel reservation");
                }
                return Ok("Reservation has been canceled");
            }
            else if (currentReservationStatus == _canceledReservationStatus)
            {
                return BadRequest("Reservation is already canceled");
            }
            else
            {
                return BadRequest("Reservation can't be canceled because is rated");
            }
        }

        /// <summary>
        /// Updates reservation data, this endpoint gives grade to reservation.
        /// </summary>
        /// <param name="clientId">Client id</param>
        /// <param name="reservationId">Reservation id</param>
        /// <param name="grade">Grade is query string</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Client.
        /// </remarks>
        [HttpPut("{clientId}/reservation/{reservationId}/rate")]
        [Authorize(Roles = UserRolesService.Client)]
        public async Task<IActionResult> RateReserationBy(int clientId, int reservationId, int grade)
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

            var clientIdentity = HttpContext.User.Identity as ClaimsIdentity;
            bool isClientClaimsValid = _jwtService.ValidateClientClaims(clientIdentity, clientId);
            if (!isClientClaimsValid)
            {
                return StatusCode(403, "Unauthorized access or jwt doesn't contains required claims");
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
            if (currentReservationStatus == _confirmedReservationStatus)
            {
                reservationDetails.ReservationGrade = grade;
                reservationDetails.Status = _ratedReservationStatus;

                bool isUpdated = await _clientApiService.UpdateReservationByClientIdAsync(clientId, reservationDetails);
                if (!isUpdated)
                {
                    return BadRequest("Unable to rate reservation");
                }
                return Ok("Reservation has been rated");
            }
            else if (currentReservationStatus == _ratedReservationStatus)
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
