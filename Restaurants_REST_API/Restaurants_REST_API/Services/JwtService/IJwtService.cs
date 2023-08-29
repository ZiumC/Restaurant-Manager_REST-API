using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.DatabaseModel;

namespace Restaurants_REST_API.Services.JwtService
{
    public interface IJwtService
    {
        /// <summary>
        /// Method generate refresh token. Length of token depends on given length in appsettings.json 
        /// </summary>
        /// <returns>Refresh token.</returns>
        public string GenerateRefreshToken();

        /// <summary>
        /// Method generate access token for user. Token contains user claims and token is signed by server.
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Signed access token.</returns>
        public string GenerateAccessTokenForUser(User user);

        /// <summary>
        /// Method valids passed pair of access and refresh token.
        /// </summary>
        /// <param name="jwt">Pair of access token and refresh token.</param>
        /// <returns>True if token is valid or false  when token isn't valid.</returns>
        public bool ValidateJwt(PostJwtDTO jwt);
    }
}
