using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class ComplaintApiService : IComplaintApiService
    {
        private readonly MainDbContext _context;

        public ComplaintApiService(MainDbContext context)
        {
            _context = context;
        }
        public Task<List<GetComplaintDTO>?> GetNewComplaintsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<List<GetComplaintDTO>?> GetPendingComplaintsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetComplaintDTO?> GetComplaintDetailsByComplaintIdAsync(int complaintId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateComplaintStatusByComplaintId(int complaint, string status)
        {
            throw new NotImplementedException();
        }
    }
}
