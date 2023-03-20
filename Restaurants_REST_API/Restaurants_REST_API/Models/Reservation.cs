using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Restaurants_REST_API.Models
{
    public class Reservation
    {
        [Key]
        public int IdReservation { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public int TableNumber { get; set; }


        public int IdClient { get; set; }

        [ForeignKey(nameof(IdClient))]
        public virtual Client Clients { get; set; }


        public int IdRestauration { get; set; }

        [ForeignKey(nameof(IdRestauration))]
        public virtual Restaurant Restaurant { get; set; }
    }
}
