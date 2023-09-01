
namespace Restaurants_REST_API.DAOs
{
    public class RestaurantDAO
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public decimal? BonusBudget { get; set; }
        public AddressDAO Address { get; set; }
    }
}
