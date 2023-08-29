using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.DatabaseModel;

namespace Restaurants_REST_API.Services.JwtService
{
    public interface IJwtService
    {
        public string GenerateRefreshToken();
        public string GenerateAccessTokenForUser(User user);
        public bool ValidateJwt(PostJwtDTO jwt);
    }
}
