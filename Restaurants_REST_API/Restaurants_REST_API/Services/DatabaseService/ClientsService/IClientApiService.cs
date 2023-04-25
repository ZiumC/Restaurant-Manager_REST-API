using Restaurants_REST_API.DTOs;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public interface IClientApiService
    {
        public Task<ClientDTO> GetClientDataByIdAsync(int id);
        public Task<ReservationDTO> GetReservationDetailsByIdAsync(int id);
        public Task<IEnumerable<int>> GetAllReservationsIdAsync();
        public Task<IEnumerable<ReservationDTO?>> GetAllReservationsAsync();
        public Task<bool> AddNewReservationAsync(ReservationDTO newReserwation);
        public Task<bool> AddNewComplainAsync(ComplainDTO newComplain);
        public Task<bool> UpdateReservationByIdAsync(int id);
    }
}
