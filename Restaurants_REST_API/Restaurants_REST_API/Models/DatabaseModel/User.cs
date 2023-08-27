using Restaurants_REST_API.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models.DatabaseModel
{
    public class User
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int LoginAttempts { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? DateBlockedTo { get; set; }


        public int? IdClient { get; set; }
        [ForeignKey(nameof(IdClient))]
        public Client? Client { get; set; }

        public int? IdEmployee { get; set; }
        [ForeignKey(nameof(IdEmployee))]
        public Employee? Employee { get; set; }
    }
}
