using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostOrPutDTO
{
    public class PostAddressDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Street { get; set; }
        [Required]
        [MaxLength(5)]
        [MinLength(1)]
        public string BuildingNumber { get; set; }
        [MaxLength(5)]
        public string? LocalNumber { get; set; }
    }
}
