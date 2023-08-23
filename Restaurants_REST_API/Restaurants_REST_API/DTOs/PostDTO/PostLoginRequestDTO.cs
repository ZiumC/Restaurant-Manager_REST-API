using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTO
{
    public class PostLoginRequestDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string LoginOrEmail { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
