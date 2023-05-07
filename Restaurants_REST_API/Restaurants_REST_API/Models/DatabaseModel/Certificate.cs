namespace Restaurants_REST_API.Models.Database
{
    public class Certificate
    {
        public int IdCertificate { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeeCertificate> EmployeeCertificates { get; set; }
    }
}
