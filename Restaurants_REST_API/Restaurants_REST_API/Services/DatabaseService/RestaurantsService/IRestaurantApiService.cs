using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<Restaurant?> GetRestaurantSimpleDataByIdAsync(int restaurantId);
        public Task<GetRestaurantDTO?> GetRestaurantDetailedDataAsync(int restaurantId);
        public Task<IEnumerable<GetRestaurantDTO>?> GetAllRestaurantsAsync();
        public Task<IEnumerable<EmployeeRestaurant>?> GetHiredEmployeesInRestaurantsAsync();
        public Task<IEnumerable<GetEmployeeTypeDTO>?> GetEmployeeTypesAsync();
        public Task<Dish?> GetDishSimpleDataByIdAsync(int dishId);
        public Task<IEnumerable<Dish>?> GetAllDishesAsync();
        public Task<IEnumerable<GetDishDTO>?> GetAllDishesWithRestaurantsAsync();
        public Task<IEnumerable<RestaurantDish>?> GetRestaurantDishesByRestaurantIdAsync(int restaurantId);
        public Task<bool> AddNewRestaurantAsync(PostRestaurantDTO newRestaurantData, int ownerTypeId);
        public Task<bool> AddNewDishToRestaurantsAsync(PostDishDTO newDish);
        public Task<bool> AddNewEmployeeToRestaurantAsync(int empId, int typeId, int restaurantId, bool isSupervisorInRestaurant);
        public Task<bool> UpdateRestaurantDataAsync(int restaurantId, PutRestaurantDTO restaurantData);
        public Task<bool> UpdateDishDataAsync(int dishId, PutDishDTO dishData);
        public Task<bool> UpdateEmployeeTypeAsync(int empId, int typeId, int restaurantId, bool isSupervisorInRestaurant);
        public Task<bool> DeleteDishAsync(Dish dish);
        public Task<bool> DeleteDishFromRestaurantAsync(int restaurantId, int dishId);
        public Task<bool> DeleteEmployeeFromRestaurantAsync(int empId, int restaurantId);
    }

}
