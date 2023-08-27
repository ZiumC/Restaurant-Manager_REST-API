using System.Security.Cryptography;

namespace Restaurants_REST_API.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly int _refreshTokenLength;
        public JwtService(IConfiguration config)
        {
            _config = config;
            try
            {
                _refreshTokenLength = int.Parse(_config["ApplicationSettings:JwtSettings:RefreshTokenLength"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string GenerateRefreshToken()
        {
            var refreshToken = "";
            using (var genNumbers = RandomNumberGenerator.Create())
            {
                byte[] array = new byte[_refreshTokenLength];
                genNumbers.GetBytes(array);
                refreshToken = Convert.ToBase64String(array);
            }

            return refreshToken;
        }

        public string GenerateAccessTokenForUserLogin(string userLogin)
        {
            throw new NotImplementedException();
        }

    }
}
