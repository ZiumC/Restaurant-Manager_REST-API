﻿using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostOrPutDTO
{
    public class EmployeeToRestaurantDTO
    {
        [Required]
        public int IdEmployee { get; set; }

        [Required]
        public int IdRestaurant { get; set; }

        [Required]
        public int IdEmployeeType { get; set; }
    }
}
