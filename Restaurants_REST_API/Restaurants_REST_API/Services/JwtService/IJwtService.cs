using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.JwtService
{
    public interface IJwtService
    {
        public string GenerateRefreshToken();
        public string GenerateAccessTokenForUserLogin(string userLogin, string userRole);
        public bool ValidateJwt(PostJwtDTO jwt);
    }
}
