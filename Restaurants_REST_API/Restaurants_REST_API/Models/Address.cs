using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.Models
{
    public class Address
    {
        public int IdAddress { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string NoBuilding { get; set; }

        public string? NoLocal { get; set; }


        public virtual ICollection<Restaurant> Restaurants { get; set;}
        public virtual ICollection<Employee> Employees { get; set;}
    }
}
