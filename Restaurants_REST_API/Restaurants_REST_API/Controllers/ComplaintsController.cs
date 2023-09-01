using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Utils.UserUtility;
using Restaurants_REST_API.Utils.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IComplaintApiService _complaintsApiService;
        private readonly IConfiguration _config;
        private readonly string _newComplaintStatus;
        private readonly string _pendingComplaintStatus;
        private readonly string _acceptedComplaintStatus;
        private readonly string _rejectedComplaintStatus;
        private readonly string _considerAction;
        private readonly string _acceptAction;
        private readonly string _rejectAction;

        public ComplaintsController(IRestaurantApiService restaurantsApiService, IComplaintApiService complaintsApiService, IConfiguration config)
        {
            _restaurantsApiService = restaurantsApiService;
            _complaintsApiService = complaintsApiService;
            _config = config;

            _newComplaintStatus = _config["ApplicationSettings:ComplaintStatus:New"];
            _pendingComplaintStatus = _config["ApplicationSettings:ComplaintStatus:Pending"];
            _acceptedComplaintStatus = _config["ApplicationSettings:ComplaintStatus:Accepted"];
            _rejectedComplaintStatus = _config["ApplicationSettings:ComplaintStatus:Rejected"];

            _considerAction = _config["ApplicationSettings:ComplaintActions:Consider"];
            _acceptAction = _config["ApplicationSettings:ComplaintActions:Accept"];
            _rejectAction = _config["ApplicationSettings:ComplaintActions:Reject"];

            try 
            {
                if (string.IsNullOrEmpty(_newComplaintStatus))
                {
                    throw new Exception("Complaint status (NEW) can't be empty");
                }

                if (string.IsNullOrEmpty(_pendingComplaintStatus))
                {
                    throw new Exception("Complaint status (PENDING) can't be empty");
                }

                if (string.IsNullOrEmpty(_acceptedComplaintStatus))
                {
                    throw new Exception("Complaint status (ACCEPTED) can't be empty");
                }

                if (string.IsNullOrEmpty(_rejectedComplaintStatus))
                {
                    throw new Exception("Complaint status (REJECTED) can't be empty");
                }

                if (string.IsNullOrEmpty(_considerAction))
                {
                    throw new Exception("Action (CONSIDER) can't be empty");
                }

                if (string.IsNullOrEmpty(_acceptAction))
                {
                    throw new Exception("Action (ACCEPT) can't be empty");
                }

                if (string.IsNullOrEmpty(_rejectAction))
                {
                    throw new Exception("Action (REJECT) can't be empty");
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }   

        /// <summary>
        /// Returns client, client reservations and complaint data based on complaint status from all restaurants.
        /// </summary>
        /// <param name="status">Complaint status could be: NEW, PENDING, ACCEPTED, REJECTED</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> GetComplains(string status)
        {
            IEnumerable<string> availableStatuses = new List<string>
            {
                _newComplaintStatus,
                _pendingComplaintStatus,
                _acceptedComplaintStatus,
                _rejectedComplaintStatus
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

        /// <summary>
        /// Updates complaint status based on action.
        /// </summary>
        /// <param name="complaintId">Complaint id</param>
        /// <param name="action">Action could be: consider, accept, reject</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        /*
         * This method is made like this because it is unnecessary to make 
         * a lot of endpoints which business logic is very similar.
         */
        [HttpPut("{complaintId}/update")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> UpdateComplaintStatus(int complaintId, string action)
        {

            if (!GeneralValidatorUtility.isIntNumberGtZero(complaintId))
            {
                return BadRequest($"Complaint id={complaintId} is invalid");
            }

            IEnumerable<string> availableActionsForEndpoint = new List<string>
            {
                _considerAction,
                _acceptAction,
                _rejectAction
            };

            action = action.ToLower();
            if (!availableActionsForEndpoint.Contains(action))
            {
                return BadRequest("Action for complaint is invalid");
            }

            var complaint = await _complaintsApiService.GetComplaintByIdAsync(complaintId);
            if (complaint == null)
            {
                return NotFound("Complaint not found");
            }

            return await UpdateComplaintUsingAction(action, complaint);
        }

        private async Task<IActionResult> UpdateComplaintUsingAction(string action, GetComplaintDTO complaint)
        {
            string statusToUpdate = _newComplaintStatus;
            string currentComplaintStatus = complaint.Status;

            if (action == _considerAction)
            {
                if (currentComplaintStatus == _newComplaintStatus)
                {
                    statusToUpdate = _pendingComplaintStatus;
                }
                else if (currentComplaintStatus == _pendingComplaintStatus)
                {
                    return BadRequest($"Complaint status is {currentComplaintStatus} already");
                }
                else 
                {
                    return BadRequest($"Unable to update complaint status to {_pendingComplaintStatus} because current status is {currentComplaintStatus}");
                }
            }
            else if (action == _acceptAction)
            {
                if (currentComplaintStatus == _pendingComplaintStatus)
                {
                    statusToUpdate = _acceptedComplaintStatus;
                }
                else if (currentComplaintStatus == _acceptedComplaintStatus)
                {
                    return BadRequest($"Complaint status is {currentComplaintStatus} already");
                }
                else
                {
                    return BadRequest($"Unable to update complaint status to {_acceptedComplaintStatus} because current status is {currentComplaintStatus}");
                }
            }
            else
            {
                if (currentComplaintStatus == _pendingComplaintStatus)
                {
                    statusToUpdate = _rejectedComplaintStatus;
                }
                else if (currentComplaintStatus == _rejectedComplaintStatus)
                {
                    return BadRequest($"Complaint status is {currentComplaintStatus} already");
                }
                else
                {
                    return BadRequest($"Unable to update complaint status to {_rejectedComplaintStatus} because current status is {currentComplaintStatus}");
                }
            }

            bool isComplaintUpdated = await _complaintsApiService.UpdateComplaintStatusByIdAsync(complaint.IdComplaint, statusToUpdate);
            if (!isComplaintUpdated)
            {
                return BadRequest("Unable to update complaint status");
            }

            return Ok($"Complaint status is {statusToUpdate} now");

        }
    }
}
