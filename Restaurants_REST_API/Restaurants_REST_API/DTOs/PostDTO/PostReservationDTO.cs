using Restaurants_REST_API.DTOs.GetDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTO
{
    public class PostReservationDTO
    {
        [Required]
        public DateTime ReservationDate { get; set; }
        [Required]
        public int HowManyPeoples { get; set; }
        [Required]
        public int IdRestaurant { get; set; }
    }
}
