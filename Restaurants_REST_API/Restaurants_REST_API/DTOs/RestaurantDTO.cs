using Restaurants_REST_API.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class RestaurantDTO
    {
        public int IdRestaurant { get; set; }
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
        public List<Dish>? RestaurantDishes { get; set; }
        public List<RestaurantWorkersDTO>? RestaurantWorkers { get; set; }
        public List<ReservationDTO>? RestaurantReservations { get; set; }
        public List<ComplainDTO>? RestaurantComplains { get; set; }
    }
}
