namespace Restaurants_REST_API.DTOs
{
    public class AddressDTO
    {
        public int IdAddress { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NoBuilding { get; set; }
        public string? NoLocal { get; set; }
    }
}
