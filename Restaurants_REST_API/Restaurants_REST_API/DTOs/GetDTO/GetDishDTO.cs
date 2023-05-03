namespace Restaurants_REST_API.DTOs.GetDTO
{
    public class GetDishDTO
    {
        public int IdDish { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<GetSimpleRestaurantDTO>? Restaurants { get; set; }
    }
}
