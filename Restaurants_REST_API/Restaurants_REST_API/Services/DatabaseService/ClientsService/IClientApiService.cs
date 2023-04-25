using Restaurants_REST_API.DTOs;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public interface IClientApiService
    {
        public Task<ClientDTO?> GetClientDataByIdAsync(int reservationId);
        public Task<ReservationDTO> GetReservationDetailsByIdAsync(int reservationId);
        public Task<IEnumerable<int>> GetAllReservationsIdAsync();
        public Task<IEnumerable<ReservationDTO?>> GetAllReservationsByClientIdAsync(int clientId);
        public Task<bool> AddNewReservationByRestaurantIdAsync(int restaurantId, ReservationDTO newReserwation);
        public Task<bool> AddNewComplainByReservationIdAsync(int reservationId, ComplainDTO newComplain);
        public Task<bool> UpdateReservationByIdAsync(int reservationId);
    }
}
