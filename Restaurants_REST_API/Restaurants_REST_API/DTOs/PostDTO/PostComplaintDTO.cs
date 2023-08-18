using Restaurants_REST_API.DTOs.GetDTOs;
using System.ComponentModel.DataAnnotations;

namespace Restaurants_REST_API.DTOs.PostDTO
{
    public class PostComplaintDTO
    {
        [Required]
        public string Message { get; set; }
    }
}
