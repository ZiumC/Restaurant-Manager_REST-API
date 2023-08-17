namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetClientDataDTO
    {
        public int IdClient { get; set; }
        public string Name { get; set; }
        public string IsBusinessman { get; set; }
        public List<GetReservationDTO>? ClientReservations { get; set; }
    }
}
