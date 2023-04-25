using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<Restaurant?> GetBasicRestaurantDataByIdAsync(int restaurantId);
        public Task<RestaurantDTO> GetRestaurantDetailsByIdAsync(int restaurantId);
        public Task<IEnumerable<RestaurantDTO?>> GetAllRestaurantsAsync();
        //public Task<IEnumerable<ReservationDTO>?> GetAllReservationsAsync();
        public Task<bool> AddNewRestaurantAsync(RestaurantDTO newRestaurant);

        public Task<bool> AddNewDishToRestaurantsAsync(DishDTO newDish);
        public Task<bool> HireNewEmployeeAsync(EmployeeHiredDTO employeeHired);
    }

}
