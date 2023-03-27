namespace Restaurants_REST_API.DTOs
{
    public class ClientDTO
    {
        public int IdClient { get; set; }
        public string Name { get; set; }
        public string IsBusinessman { get; set; }
        public List<ReservationDTO>? ClientReservations { get; set; }
    }
}
