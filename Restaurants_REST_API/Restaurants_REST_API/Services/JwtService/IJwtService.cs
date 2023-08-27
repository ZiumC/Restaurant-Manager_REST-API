namespace Restaurants_REST_API.Services.JwtService
{
    public interface IJwtService
    {
        public string GenerateRefreshToken();
        public string GenerateAccessTokenForUserLogin(string userLogin);
    }
}
