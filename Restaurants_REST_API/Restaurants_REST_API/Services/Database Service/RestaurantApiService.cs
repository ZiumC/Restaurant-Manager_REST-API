using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class RestaurantApiService : IRestaurantApiService
    {

        private readonly MainDbContext _context;

        public RestaurantApiService(MainDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Restaurant> GetRestaurantByIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<Complain> GetComplainsByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Restaurant?> CheckIfRestaurantExistByIdAsync(int restaurantId)
        {
            return await _context.Restaurants.Where(e => e.IdRestaurant == restaurantId).FirstOrDefaultAsync();
        }
    }
}
