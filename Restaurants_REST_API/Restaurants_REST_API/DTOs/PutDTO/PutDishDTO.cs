using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PutDTO
{
    public class PutDishDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
