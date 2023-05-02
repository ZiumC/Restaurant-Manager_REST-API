using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostOrPutDTO
{
    public class PostRestaurantDTO
    {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Status { get; set; }
        public decimal? BonusBudget { get; set; }
        [Required]
        public PostAddressDTO Address { get; set; }
    }
}
