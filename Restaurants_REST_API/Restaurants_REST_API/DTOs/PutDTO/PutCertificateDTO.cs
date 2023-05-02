using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PutDTO
{
    public class PutCertificateDTO
    {
        [Required]
        [MaxLength(125)]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
