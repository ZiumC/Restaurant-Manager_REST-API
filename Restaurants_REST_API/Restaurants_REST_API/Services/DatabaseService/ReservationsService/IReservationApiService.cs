using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IReservationApiService
    {
        public Task<IEnumerable<GetReservationDTO>?> GetAllReservationsAsync();
        public Task<GetReservationDTO?> GetReservationByIdAsync(int reservationId);
        public Task<IEnumerable<GetReservationDTO>?> GetReservationsByRestaurantIdAsync(int restaurantId);
        public Task<GetClientDTO?> GetReservationsByClientIdAsync(int clientId);
    }
}
