using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostOrPutDTO
{
    public class DishDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public List<int> IdRestaurants { get; set; }
    }
}
