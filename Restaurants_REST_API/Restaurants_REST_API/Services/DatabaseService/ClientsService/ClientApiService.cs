using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public class ClientApiService : IClientApiService
    {
        private readonly MainDbContext _context;

        public ClientApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CancelReservationByClientIdReservationIdAsync(int clientId, int reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmReservationByClientIdReservationIdAsync(int clientId, int reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetReservationDTO>?> GetAllReservationsDataByClientIdAsync(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task<GetReservationDTO?> GetReservationDetailsByReservationIdAsync(int reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> MakeComplaintByClientIdAsync(int clientId, PostComplaintDTO newComplaint)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> MakeReservationAsync(PostReservationDTO newReservation)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RateReservationByReservationIdAsync(int reserationId, int reservationGrade)
        {
            throw new NotImplementedException();
        }
    }
}
