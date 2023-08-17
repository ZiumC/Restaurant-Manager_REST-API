using Restaurants_REST_API.DTOs.GetDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTO
{
    public class PostComplaintDTO
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public int ReservationId { get; set; }
        [Required]
        public int RestaurantId { get; set; }
    }
}
