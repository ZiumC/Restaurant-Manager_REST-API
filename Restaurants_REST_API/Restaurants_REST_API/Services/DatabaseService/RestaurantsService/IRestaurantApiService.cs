using Restaurants_REST_API.DAOs;
using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
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
        public Task<bool> AddNewRestaurantAsync(RestaurantDAO newRestaurantData, int ownerTypeId);
        public Task<bool> AddNewDishToRestaurantsAsync(DishDAO newDish, IEnumerable<int> restaurantsId);
        public Task<bool> AddExistingDishToRestaurantAsync(int dishId, int restaurantId);
        public Task<bool> AddNewEmployeeToRestaurantAsync(int empId, int typeId, int restaurantId, bool isSupervisorInRestaurant);
        public Task<bool> UpdateRestaurantDataAsync(int restaurantId, RestaurantDAO restaurantData);
        public Task<bool> UpdateDishDataAsync(int dishId, DishDAO dishData);
        public Task<bool> UpdateEmployeeTypeAsync(int empId, int typeId, int restaurantId, bool isSupervisorInRestaurant);
        public Task<bool> DeleteDishAsync(int dishId);
        public Task<bool> DeleteDishFromRestaurantAsync(int restaurantId, int dishId);
        public Task<bool> DeleteEmployeeFromRestaurantAsync(int empId, int restaurantId);
    }

}
