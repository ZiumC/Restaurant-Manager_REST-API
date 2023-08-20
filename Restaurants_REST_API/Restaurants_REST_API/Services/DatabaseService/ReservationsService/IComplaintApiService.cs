using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IComplaintApiService
    {
        public Task<List<GetComplaintDTO>?> GetNewComplaintsAsync();
        public Task<List<GetComplaintDTO>?> GetPendingComplaintsAsync();
        public Task<GetComplaintDTO?> GetComplaintDetailsByComplaintIdAsync(int complaintId);
        public Task<bool> UpdateComplaintStatusByComplaintId(int complaint, string status);
    }
}
