using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        public Task<Restaurant> GetRestaurantByIdAsync(int restaurantId);

        public Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        public Task<Reservation> GetReservationByIdAsync(int reservationId);
    }
}
