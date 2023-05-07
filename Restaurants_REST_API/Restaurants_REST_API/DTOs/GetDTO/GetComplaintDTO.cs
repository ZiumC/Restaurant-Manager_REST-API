namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetComplaintDTO
    {
        public int IdComplaint { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
