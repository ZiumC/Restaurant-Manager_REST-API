using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.Models.Database
{
    public class Address
    {
        public int IdAddress { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string? LocalNumber { get; set; }


        public virtual ICollection<Restaurant> Restaurants { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
