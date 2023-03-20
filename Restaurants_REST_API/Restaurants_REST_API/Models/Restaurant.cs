using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models
{
    public class Restaurant
    {
        [Key]
        public int IdRestaurant { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int IdAddress { get; set; }

        [ForeignKey(nameof(IdAddress))]
        public virtual Address Address { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }  
    }
}
