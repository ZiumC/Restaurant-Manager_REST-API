using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetEmployeeDTO
    {
        public int IdEmployee { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public decimal Salary { get; set; }
        public decimal BonusSalary { get; set; }
        public DateTime HiredDate { get; set; }
        public DateTime? FirstPromotionChefDate { get; set; }
        public string IsOwner { get; set; }
        public GetAddressDTO Address { get; set; }
        public List<GetCertificateDTO?>? Certificates { get; set; }
    }
}
