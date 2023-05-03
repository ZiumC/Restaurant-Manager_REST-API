namespace Restaurants_REST_API.DTOs.GetDTO
{
    public class GetSimpleRestaurantDTO
    {
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal? BonusBudget { get; set; }
    }
}
