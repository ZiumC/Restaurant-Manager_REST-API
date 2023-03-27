﻿using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IRestaurantApiService
    {
        public Task<IEnumerable<RestaurantDTO>> GetAllRestaurantsAsync();
        public Task<Restaurant> GetRestaurantByIdAsync(int restaurantId);
        public Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        public Task<Reservation> GetReservationByIdAsync(int reservationId);
        public Task<Complain> GetComplainsByRestaurantIdAsync(int restaurantId);
        public Task<Restaurant?> CheckIfRestaurantExistByIdAsync(int restaurantId);
    }
}
