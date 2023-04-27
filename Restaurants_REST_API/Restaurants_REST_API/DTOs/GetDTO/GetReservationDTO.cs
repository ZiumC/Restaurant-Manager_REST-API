using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetReservationDTO
    {
        public int IdReservation { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; }
        public int? ReservationGrade { get; set; }
        [Required]
        public int HowManyPeoples { get; set; }
        public GetComplainDTO? ReservationComplain { get; set; }
    }
}
