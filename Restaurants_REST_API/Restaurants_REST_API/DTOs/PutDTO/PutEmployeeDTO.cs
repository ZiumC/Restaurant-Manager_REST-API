using Restaurants_REST_API.DTOs.PostOrPutDTO;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PutDTO
{
    public class PutEmployeeDTO
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
        public PutAddressDTO Address { get; set; }
    }
}
