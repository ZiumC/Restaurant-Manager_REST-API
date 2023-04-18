using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class EmployeeDTO
    {
        public int IdEmployee { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string PESEL { get; set; }
        [Required]
        public decimal Salary { get; set; }
        public decimal BonusSalary { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime? FirstPromotionChefDate { get; set; }
        [Required]
        public string IsOwner { get; set; }
        [Required]
        public AddressDTO Address { get; set; }
        public List<CertificateDTO>? Certificates { get; set;}
    }
}
