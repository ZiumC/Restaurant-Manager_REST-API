namespace Restaurants_REST_API.DTOs
{
    public class ReservationDTO
    {
        public int IdReservation { get; set; }

        public DateTime ReservationDate { get; set; }

        public string Status { get; set; }
        public int? GradeOfReservation { get; set; }

        public int TableNumber { get; set; }
    }
}
