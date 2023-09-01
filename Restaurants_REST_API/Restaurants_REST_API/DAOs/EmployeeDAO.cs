using Restaurants_REST_API.DTOs.PostOrPutDTO;

namespace Restaurants_REST_API.DAOs
{
    public class EmployeeDAO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public decimal Salary { get; set; }
        public decimal BonusSalary { get; set; }
        public AddressDAO Address { get; set; }
        public IEnumerable<CertificateDAO>? Certificates { get; set; }
    }
}
