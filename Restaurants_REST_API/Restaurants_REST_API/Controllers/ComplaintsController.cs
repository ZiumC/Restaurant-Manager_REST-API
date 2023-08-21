using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.CustomersService;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IClientApiService _clientApiService;
        private readonly IComplaintApiService _complaintsApiService;
        private readonly IConfiguration _config;

        public ComplaintsController(IRestaurantApiService restaurantsApiService, IComplaintApiService complaintsApiService, IClientApiService clientApiService, IConfiguration config)
        {
            _restaurantsApiService = restaurantsApiService;
            _clientApiService = clientApiService;
            _complaintsApiService = complaintsApiService;
            _config = config;
        }

        /// <summary>
        /// Returns client, client reservations and complaint data based on complaint status from all restaurants.
        /// </summary>
        /// <param name="status">Status could be: NEW, PENDING, ACCEPTED, REJECTED</param>
        [HttpGet]
        public async Task<IActionResult> GetComplainsByStatus(string status)
        {
            IEnumerable<string> availableStatuses = new List<string>
            {
                _config["ApplicationSettings:ComplaintStatus:New"],
                _config["ApplicationSettings:ComplaintStatus:Pending"],
                _config["ApplicationSettings:ComplaintStatus:Accepted"],
                _config["ApplicationSettings:ComplaintStatus:Rejected"]
            };

            if (!availableStatuses.Contains(status))
            {
                return BadRequest("Complaint status is invalid");
            }

            var rawComplaints = await _complaintsApiService.GetClientComplaintsByStatusAsync(status);
            if (rawComplaints == null || rawComplaints.Count() == 0)
            {
                return NotFound("Complains not found");
            }

            rawComplaints.RemoveAll(rc => rc.ClientReservations == null || rc.ClientReservations.Count() == 0);
            List<GetClientDataDTO>? filteredComplaints = rawComplaints;
            if (filteredComplaints == null || filteredComplaints.Count() == 0)
            {
                return NotFound("Complains not found");
            }

            var restaurantsDetails = await _restaurantsApiService.GetAllRestaurantsAsync();
            if (restaurantsDetails == null || restaurantsDetails.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }

            var result = rawComplaints.Select(c => new
            {
                ClientName = c.Name,
                IsBusinessMan = c.IsBusinessman,
                ClientReservations = c.ClientReservations?.Select(cr => new
                {
                    ResevtaionDate = cr.ReservationDate,
                    ReservationStatus = cr.Status,
                    ReservationGrade = cr.ReservationGrade,
                    HowManyPeoples = cr.HowManyPeoples,
                    RestaurantName = restaurantsDetails
                    .Where(rd => rd.RestaurantReservations
                    .Any(a => a.IdReservation == cr.IdReservation))
                    .FirstOrDefault()?.Name,
                    Complaint = cr.ReservationComplaint
                })
            });

            return Ok(result);
        }

        [HttpPut("{complaintId}/update")]
        public async Task<IActionResult> UpdateComplaintStatus(int complaintId, string action)
        {

            if (!GeneralValidator.isNumberGtZero(complaintId))
            {
                return BadRequest($"Complaint id={complaintId} is invalid");
            }

            IEnumerable<string> availableActionsForEndpoint = new List<string>
            {
                "consider",
                "accept",
                "reject"
            };

            action = action.ToLower();
            if (!availableActionsForEndpoint.Contains(action))
            {
                return BadRequest("Action for complaint is invalid");
            }

            var complaint = await _complaintsApiService.GetComplaintByComplaintIdAsync(complaintId);
            if (complaint == null)
            {
                return NotFound("Complaint not found");
            }

            string currentComplaintStatus = complaint.Status;
            string newStatus = _config["ApplicationSettings:ComplaintStatus:New"];
            string pendingStatus = _config["ApplicationSettings:ComplaintStatus:Pending"];
            if (action == availableActionsForEndpoint.ElementAt(0))
            {
                if (currentComplaintStatus == newStatus)
                {
                    bool isComplaintUpdated = await _complaintsApiService.UpdateComplaintStatusByComplaintIdAsync(complaintId, pendingStatus);

                    if (!isComplaintUpdated)
                    {
                        return BadRequest("Unable to update complaint status");
                    }

                    return Ok($"Complaint status is {pendingStatus} now");
                }
                else if (currentComplaintStatus == pendingStatus)
                {
                    return BadRequest($"Complaint status is {pendingStatus} already");
                }

                return BadRequest("Unable to update complaint status because is accepted or rejected");
            }
            else if (action == availableActionsForEndpoint.ElementAt(1))
            {
                string acceptedStatus = _config["ApplicationSettings:ComplaintStatus:Accepted"];
                if (currentComplaintStatus == pendingStatus)
                {
                    bool isComplaintUpdated = await _complaintsApiService.UpdateComplaintStatusByComplaintIdAsync(complaintId, acceptedStatus);

                    if (!isComplaintUpdated)
                    {
                        return BadRequest("Unable to update complaint status");
                    }

                    return Ok($"Complaint status is {acceptedStatus} now");
                }
                else if (currentComplaintStatus == acceptedStatus)
                {
                    return BadRequest($"Complaint status is {acceptedStatus} already");
                }

                return BadRequest("Unable to update complaint status because is new or rejected");
            }
            else
            {
                string rejectedStatus = _config["ApplicationSettings:ComplaintStatus:Rejected"];
                if (currentComplaintStatus == pendingStatus)
                {
                    bool isComplaintUpdated = await _complaintsApiService.UpdateComplaintStatusByComplaintIdAsync(complaintId, rejectedStatus);

                    if (!isComplaintUpdated)
                    {
                        return BadRequest("Unable to update complaint status");
                    }

                    return Ok($"Complaint status is {rejectedStatus} now");
                }
                else if (currentComplaintStatus == rejectedStatus)
                {
                    return BadRequest($"Complaint status is {rejectedStatus} already");
                }

                return BadRequest("Unable to update complaint status because is new or accepted");
            }


        }

        //[HttpPut("{complaintId}/accept")]
        //public async Task<IActionResult> UpdateComplaintStatusToAccepted(int complaintId)
        //{

        //    if (!GeneralValidator.isNumberGtZero(complaintId))
        //    {
        //        return BadRequest($"Complaint id={complaintId} is invalid");
        //    }

        //    var complaint = await _complaintsApiService.GetComplaintByComplaintIdAsync(complaintId);
        //    if (complaint == null)
        //    {
        //        return NotFound("Complaint not found");
        //    }

        //    string currentComplaintStatus = complaint.Status;
        //    string pendingStatus = _config["ApplicationSettings:ComplaintStatus:Pending"];
        //    string acceptedStatus = _config["ApplicationSettings:ComplaintStatus:Accepted"];
        //    if (currentComplaintStatus == pendingStatus)
        //    {
        //        bool isComplaintUpdated = await _complaintsApiService.UpdateComplaintStatusByComplaintIdAsync(complaintId, acceptedStatus);

        //        if (!isComplaintUpdated)
        //        {
        //            return BadRequest("Unable to update complaint status");
        //        }

        //        return Ok($"Complaint status is {acceptedStatus} now");
        //    }

        //    return BadRequest("Unable to update complaint status because is new or rejected");
        //}

    }
}
