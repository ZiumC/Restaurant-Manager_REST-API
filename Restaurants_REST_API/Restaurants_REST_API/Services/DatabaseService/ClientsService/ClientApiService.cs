using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public class ClientApiService : IClientApiService
    {
        private readonly MainDbContext _context;

        public ClientApiService(MainDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<int>> GetAllReservationsIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetReservationDTO?>> GetAllReservationsByClientIdAsync(int clientId)
        {
            throw new NotImplementedException();
        }

        public Task<GetClientDTO?> GetClientDataByIdAsync(int reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<GetReservationDTO> GetReservationDetailsByIdAsync(int reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddNewComplainByReservationIdAsync(int reservationId, GetComplainDTO newComplain)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddNewReservationByRestaurantIdAsync(int restaurantId, GetReservationDTO newReserwation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateReservationByIdAsync(int reservationId)
        {
            throw new NotImplementedException();
        }
    }
}
