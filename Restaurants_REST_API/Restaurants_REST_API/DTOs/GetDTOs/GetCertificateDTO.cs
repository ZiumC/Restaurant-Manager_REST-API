using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.GetDTOs
{
    public class GetCertificateDTO
    {
        public int IdCertificate { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
