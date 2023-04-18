using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs
{
    public class CertificateDTO
    {
        public int IdCertificate { get; set; }
        [Required]
        [MaxLength(125)]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
