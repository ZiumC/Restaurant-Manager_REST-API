using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models.Database
{
    public class DishInRestaurant
    {
        public int IdRestaurantDish { get; set; }


        public int IdDish { get; set; }
        [ForeignKey(nameof(IdDish))]
        public virtual Dish Dish { get; set; }


        public int IdRestaurant { get; set; }
        [ForeignKey(nameof(IdRestaurant))]
        public virtual Restaurant? Restaurant { get; set; }
    }
}
