using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PutDTOs
{
    public class PutAddressDTO
    {
        [MaxLength(50)]
        [MinLength(5)]
        public string City { get; set; }
        [MaxLength(50)]
        [MinLength(5)]
        public string Street { get; set; }
        [MaxLength(5)]
        [MinLength(1)]
        public string BuildingNumber { get; set; }
        [MaxLength(5)]
        [MinLength(1)]
        public string? LocalNumber { get; set; }
    }
}
