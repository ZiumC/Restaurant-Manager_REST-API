using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IReservationApiService
    {
        public Task<IEnumerable<ReservationDTO>?> GetAllReservationsAsync();
        public Task<IEnumerable<ReservationDTO>?> GetReservationsByRestaurantIdAsync(int restaurantId);
        public Task<ComplainDTO> GetComplainsByRestaurantIdAsync(int restaurantId);
    }
}
