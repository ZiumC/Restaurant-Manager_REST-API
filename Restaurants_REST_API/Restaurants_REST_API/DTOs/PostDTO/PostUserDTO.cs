﻿using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTO
{
    public class PostUserDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Login { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Password { get; set; }
        public string PESEL { get; set; }
    }
}
