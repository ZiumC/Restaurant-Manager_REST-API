namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetComplainDTO
    {
        public int IdComplain { get; set; }
        public DateTime ComplainDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
