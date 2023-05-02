using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostOrPutDTO
{
    public class PostEmployeeDTO
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
        public PostAddressDTO Address { get; set; }
        public IEnumerable<PostCertificateDTO>? Certificates { get; set; }

    }
}
