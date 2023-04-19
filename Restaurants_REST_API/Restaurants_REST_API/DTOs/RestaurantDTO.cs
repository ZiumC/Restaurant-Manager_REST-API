using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.DTOs
{
    public class RestaurantDTO
    {
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal? BonusBudget { get; set; }
        public AddressDTO Address { get; set; }
        public List<Dish>? RestaurantDishes { get; set; }
        public List<RestaurantWorersDTO>? RestaurantWorkers { get; set; }
        public List<ReservationDTO>? RestaurantReservations { get; set; }
        public List<ComplainDTO>? RestaurantComplains { get; set; }
    }
}
