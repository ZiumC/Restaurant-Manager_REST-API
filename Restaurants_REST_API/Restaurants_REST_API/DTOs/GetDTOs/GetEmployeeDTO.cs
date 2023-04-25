using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetEmployeeDTO
    {
        public int IdEmployee { get; set; }
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
        public DateTime HiredDate { get; set; }
        public DateTime? FirstPromotionChefDate { get; set; }
        [Required]
        public string IsOwner { get; set; }
        [Required]
        public GetAddressDTO Address { get; set; }
        public List<GetCertificateDTO?>? Certificates { get; set; }
    }
}
