using Restaurants_REST_API.DTOs.GetDTOs;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IReservationApiService
    {
        public Task<IEnumerable<GetReservationDTO>?> GetAllReservationsAsync();
        public Task<GetReservationDTO?> GetReservationByIdAsync(int reservationId);
        public Task<IEnumerable<GetReservationDTO>?> GetRestaurantReservationsAsync(int restaurantId);
        public Task<GetClientDataDTO?> GetReservationsByClientIdAsync(int clientId);
    }
}
