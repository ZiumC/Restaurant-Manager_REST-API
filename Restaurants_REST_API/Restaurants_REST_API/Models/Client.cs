using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.Models
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1)]
        public string IsBusinessman { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
