namespace Restaurants_REST_API.Models
{
    public class Dish
    {
        public int IdDish { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    
        public virtual ICollection<DishInRestaurant> DishInRestaurants { get; set; }
    }
}
