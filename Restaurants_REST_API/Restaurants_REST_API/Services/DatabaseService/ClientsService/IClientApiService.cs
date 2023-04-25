using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.DatabaseService.CustomersService
{
    public interface IClientApiService
    {
        public Task<GetClientDTO?> GetClientDataByIdAsync(int reservationId);
        public Task<GetReservationDTO> GetReservationDetailsByIdAsync(int reservationId);
        public Task<IEnumerable<int>> GetAllReservationsIdAsync();
        public Task<IEnumerable<GetReservationDTO?>> GetAllReservationsByClientIdAsync(int clientId);
        public Task<bool> AddNewReservationByRestaurantIdAsync(int restaurantId, GetReservationDTO newReserwation);
        public Task<bool> AddNewComplainByReservationIdAsync(int reservationId, GetComplainDTO newComplain);
        public Task<bool> UpdateReservationByIdAsync(int reservationId);
    }
}
