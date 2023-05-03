using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<Restaurant?> GetBasicRestaurantDataByIdAsync(int restaurantId);
        public Task<GetRestaurantDTO> GetRestaurantDetailedDataAsync(Restaurant restaurant);
        public Task<IEnumerable<GetRestaurantDTO?>> GetAllRestaurantsAsync();
        public Task<IEnumerable<EmployeesInRestaurant?>> GetEmployeeInRestaurantDataByRestaurantIdAsync(int restaurantId);
        public Task<bool> AddNewRestaurantAsync(PostRestaurantDTO newRestaurant);
        public Task<bool> AddNewDishToRestaurantsAsync(PostDishDTO newDish);
        public Task<bool> HireNewEmployeeAsync(int empId, int typeId, int restaurantId);
        public Task<bool> UpdateRestaurantDataAsync(int id, Restaurant newRestaurantData);
    }

}
