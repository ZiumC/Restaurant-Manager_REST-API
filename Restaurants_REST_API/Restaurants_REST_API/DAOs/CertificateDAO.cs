using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DAOs
{
    public class CertificateDAO
    {
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
