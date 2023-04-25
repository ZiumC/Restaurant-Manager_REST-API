using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class EmployeeHiredDTO
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public string EmployeeType { get; set; }
    }
}
