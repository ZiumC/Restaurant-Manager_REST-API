using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models
{
    public class Employee
    {
        public int IdEmployee { get; set; }

        public decimal Salary { get; set; }

        public DateTime HiredDate { get; set; }

        public string IsHealthBook { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [MinLength(11)]
        public string PESEL { get; set; }

        public string IsOwner { get; set; }


        public virtual int IdAddress { get; set; }

        [ForeignKey(nameof(IdAddress))]
        public virtual Address Address { get; set; }

    }
}
