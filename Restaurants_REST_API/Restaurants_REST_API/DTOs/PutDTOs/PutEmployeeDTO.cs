using Restaurants_REST_API.DTOs.PostDTOs;
using Restaurants_REST_API.DTOs.PutDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class PutEmployeeDTO
    {
        [MaxLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [MinLength(3)]
        public string LastName { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public string PESEL { get; set; }
        public decimal Salary { get; set; }
        public decimal BonusSalary { get; set; }
        public PutAddressDTO? Address { get; set; }
        public IEnumerable<PutCertificateDTO?>? Certificates { get; set; }
    }
}
