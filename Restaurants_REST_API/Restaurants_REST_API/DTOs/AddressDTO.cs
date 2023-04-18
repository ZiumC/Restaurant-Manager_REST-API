using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class AddressDTO
    {
        public int IdAddress { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Street { get; set; }
        [Required]
        [MaxLength(5)]
        [MinLength(1)]
        public string BuildingNumber { get; set; }
        [MaxLength(5)]
        [MinLength(1)]
        public string? LocalNumber { get; set; }
    }
}
