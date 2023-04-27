using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTOs
{
    public class RestaurantDTO
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
        public AddressDTO Address { get; set; }
    }
}
