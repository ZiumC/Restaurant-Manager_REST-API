using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models
{
    public class Restaurant
    {
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string RestaurantStatus { get; set; }
        public decimal? BonusBudget { get; set; }

        public int IdAddress { get; set; }
        [ForeignKey(nameof(IdAddress))]
        public virtual Address Address { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual ICollection<EmployeesInRestaurant> RestaurantEmployees { get; set; }

        public virtual ICollection<DishInRestaurant> RestaurantDishes { get; set; } 
        
        public virtual ICollection<Complain> Complains { get; set; }
    }
}
