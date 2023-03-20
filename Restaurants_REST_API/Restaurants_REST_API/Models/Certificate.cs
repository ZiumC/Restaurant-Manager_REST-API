namespace Restaurants_REST_API.Models
{
    public class Certificate
    {
        public int IdCertificate { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeeCertificates> EmployeeCertificates { get; set; }
    }
}
