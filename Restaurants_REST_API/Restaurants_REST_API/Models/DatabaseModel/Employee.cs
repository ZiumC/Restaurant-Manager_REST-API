using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants_REST_API.Models.Database
{
    public class Employee
    {
        public int IdEmployee { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime? FirstPromotionChefDate { get; set; }
        public decimal Salary { get; set; }
        public decimal BonusSalary { get; set; }
        public string IsOwner { get; set; }
        public virtual int IdAddress { get; set; }

        [ForeignKey(nameof(IdAddress))]
        public virtual Address Address { get; set; }

        public virtual ICollection<EmployeeCertificates> EmployeeCertificates { get; set; }

        public virtual ICollection<EmployeesInRestaurant> EmployeeInRestaurant { get; set; }
    }
}
