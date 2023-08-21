using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IComplaintApiService
    {
        public Task<List<GetClientDataDTO>?> GetClientComplaintsByStatusAsync(string status);
        public Task<GetComplaintDTO?> GetComplaintByComplaintIdAsync(int complaintId);
        public Task<bool> UpdateComplaintStatusByComplaintIdAsync(int complaintId, string status);
    }
}
