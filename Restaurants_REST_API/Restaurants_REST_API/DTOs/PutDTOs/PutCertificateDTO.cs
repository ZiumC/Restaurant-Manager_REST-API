using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PutDTOs
{
    public class PutCertificateDTO
    {
        [MaxLength(125)]
        [MinLength(5)]
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
