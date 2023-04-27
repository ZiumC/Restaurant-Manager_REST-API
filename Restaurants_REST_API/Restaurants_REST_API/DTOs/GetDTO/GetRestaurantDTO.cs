using Restaurants_REST_API.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetRestaurantDTO
    {
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal? BonusBudget { get; set; }
        public GetAddressDTO Address { get; set; }
        public List<Dish>? RestaurantDishes { get; set; }
        public List<GetRestaurantWorkersDTO>? RestaurantWorkers { get; set; }
        public List<GetReservationDTO>? RestaurantReservations { get; set; }
        public List<GetComplainDTO>? RestaurantComplains { get; set; }
    }
}
