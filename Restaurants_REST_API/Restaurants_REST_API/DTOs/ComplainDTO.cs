namespace Restaurants_REST_API.DTOs
{
    public class ComplainDTO
    {
        public int IdComplain { get; set; }
        public DateTime ComplainDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
