﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.DatabaseService.CustomersService;
using Restaurants_REST_API.Utils;
using Restaurants_REST_API.Utils.ValidatorService;

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
        /// <param name="status">Complaint status could be: NEW, PENDING, ACCEPTED, REJECTED</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> GetComplainsByStatusBy(string status)
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
        public async Task<IActionResult> UpdateComplaintStatusBy(int complaintId, string action)
        {

            if (!GeneralValidatorUtility.isIntNumberGtZero(complaintId))
            {
                return BadRequest($"Complaint id={complaintId} is invalid");
            }

            IEnumerable<string> availableActionsForEndpoint = new List<string>
            {
                _config["ApplicationSettings:ComplaintActions:Consider"],
                _config["ApplicationSettings:ComplaintActions:Accept"],
                _config["ApplicationSettings:ComplaintActions:Reject"],
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

            return await UpdateComplaintByActionAsync(action, complaint);
        }

        private async Task<IActionResult> UpdateComplaintByActionAsync(string action, GetComplaintDTO complaint)
        {
            //NEED TO REFACTOR THIS!!!!!!!!!
            string newStatus = _config["ApplicationSettings:ComplaintStatus:New"];
            string pendingStatus = _config["ApplicationSettings:ComplaintStatus:Pending"];
            string acceptedStatus = _config["ApplicationSettings:ComplaintStatus:Accepted"];
            string rejectedStatus = _config["ApplicationSettings:ComplaintStatus:Rejected"];

            string considerAction = _config["ApplicationSettings:ComplaintActions:Consider"];
            string acceptAction = _config["ApplicationSettings:ComplaintActions:Accept"];

            string statusToUpdate = newStatus;
            string currentComplaintStatus = complaint.Status;

            if (action == considerAction)
            {
                if (currentComplaintStatus == newStatus)
                {
                    statusToUpdate = pendingStatus;
                }
                else if (currentComplaintStatus == pendingStatus)
                {
                    return BadRequest($"Complaint status is {currentComplaintStatus} already");
                }
                else 
                {
                    return BadRequest($"Unable to update complaint status to {pendingStatus} because current status is {currentComplaintStatus}");
                }
            }
            else if (action == acceptAction)
            {
                if (currentComplaintStatus == pendingStatus)
                {
                    statusToUpdate = acceptedStatus;
                }
                else if (currentComplaintStatus == acceptedStatus)
                {
                    return BadRequest($"Complaint status is {currentComplaintStatus} already");
                }
                else
                {
                    return BadRequest($"Unable to update complaint status to {acceptedStatus} because current status is {currentComplaintStatus}");
                }
            }
            else
            {
                if (currentComplaintStatus == pendingStatus)
                {
                    statusToUpdate = rejectedStatus;
                }
                else if (currentComplaintStatus == rejectedStatus)
                {
                    return BadRequest($"Complaint status is {currentComplaintStatus} already");
                }
                else
                {
                    return BadRequest($"Unable to update complaint status to {rejectedStatus} because current status is {currentComplaintStatus}");
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
