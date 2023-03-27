using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class EmployeeDTO
    {
        public int IdEmployee { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PESEL { get; set; }
        public decimal Salary { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime? FirstPromotionChefDate { get; set; }
        public string IsOwner { get; set; }
        public AddressDTO Address { get; set; }
        public List<CertificateDTO>? Certificates { get; set;}
    }
}
