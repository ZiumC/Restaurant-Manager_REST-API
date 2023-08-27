using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTO
{
    public class PostJwtDTO
    {
        [Required]
        [MaxLength(500)]
        public string AccessToken { get; set; }
        [Required]
        [MaxLength(1000)]
        public string RefreshToken { get; set; }
    }
}
