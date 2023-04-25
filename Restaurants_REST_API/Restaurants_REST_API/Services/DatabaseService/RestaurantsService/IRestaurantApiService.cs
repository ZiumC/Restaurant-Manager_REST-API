using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<Restaurant?> GetBasicRestaurantDataByIdAsync(int restaurantId);
        public Task<GetRestaurantDTO> GetRestaurantDetailsByIdAsync(int restaurantId);
        public Task<IEnumerable<GetRestaurantDTO?>> GetAllRestaurantsAsync();
        //public Task<IEnumerable<ReservationDTO>?> GetAllReservationsAsync();
        public Task<bool> AddNewRestaurantAsync(GetRestaurantDTO newRestaurant);

        public Task<bool> AddNewDishToRestaurantsAsync(GetDishDTO newDish);
        public Task<bool> HireNewEmployeeAsync(GetEmployeeHiredDTO employeeHired);
    }

}
