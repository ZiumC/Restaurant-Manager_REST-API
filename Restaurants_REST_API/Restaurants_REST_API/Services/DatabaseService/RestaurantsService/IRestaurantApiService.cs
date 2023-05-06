﻿using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<Restaurant?> GetBasicRestaurantDataByIdAsync(int restaurantId);
        public Task<GetRestaurantDTO> GetDetailedRestaurantDataAsync(Restaurant restaurant);
        public Task<IEnumerable<GetRestaurantDTO?>> GetAllRestaurantsAsync();
        public Task<IEnumerable<EmployeesInRestaurant?>> GetHiredEmployeesByRestaurantIdAsync(int restaurantId);
        public Task<IEnumerable<GetEmployeeTypeDTO?>> GetEmployeeTypesAsync();
        public Task<Dish?> GetBasicDishDataByIdAsync(int dishId);
        public Task<IEnumerable<DishInRestaurant?>> GetRestaurantDishesByRestaurantIdAsync(int restaurantId);
        public Task<bool> AddNewEmployeeTypeAsync(string name);
        public Task<bool> AddNewRestaurantAsync(PostRestaurantDTO newRestaurant);
        public Task<bool> AddNewDishToRestaurantsAsync(PostDishDTO newDish);
        public Task<bool> AddNewEmployeeToRestaurantAsync(int empId, int typeId, int restaurantId);
        public Task<bool> UpdateRestaurantDataAsync(int id, Restaurant newRestaurantData);
        public Task<bool> UpdateDishDataAsync(int id, Dish newDishData);
        public Task<bool> UpdateEmployeeTypeAsync(int empId, int typeId, int restaurantId);
        public Task<bool> DeleteDishAsync(Dish dishData);
        public Task<bool> DeleteDishFromRestaurantAsync(int restaurantId, int dishId);
        public Task<bool> DeleteEmployeeFromRestaurantAsync(int empId, int restaurantId);
    }

}
