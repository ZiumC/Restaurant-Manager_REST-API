using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.Models.Database
{
    public class Client
    {
        public int IdClient { get; set; }

        public string Name { get; set; }

        public string IsBusinessman { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
