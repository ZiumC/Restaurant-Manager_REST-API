using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.Models
{
    public class Address
    {
        [Key]
        public int IdAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(5)]
        public string NoBuilding { get; set; }

        [MaxLength(5)]
        public string NoLocal { get; set; }


        public virtual ICollection<Restaurant> Restaurants { get; set;}
    }
}
