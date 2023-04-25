using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public class ClientApiService : IClientApiService
    {
        private readonly MainDbContext _context;

        public ClientApiService(MainDbContext context)
        {
            _context = context;
        }

        public Task<bool> AddNewComplainAsync(ComplainDTO newComplain)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddNewReservationAsync(ReservationDTO newReserwation)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReservationDTO?>> GetAllReservationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetAllReservationsIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ClientDTO> GetClientDataByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationDTO> GetReservationDetailsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateReservationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
