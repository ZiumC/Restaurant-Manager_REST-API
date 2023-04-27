using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTOs
{
    public class EmployeeDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string PESEL { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public decimal BonusSalary { get; set; }
        [Required]
        public AddressDTO Address { get; set; }
        public IEnumerable<CertificateDTO>? Certificates { get; set; }

    }
}
