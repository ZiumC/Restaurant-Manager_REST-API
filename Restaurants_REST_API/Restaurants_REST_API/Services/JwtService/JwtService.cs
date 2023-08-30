using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Restaurants_REST_API.DTOs.PostDTO;
using Restaurants_REST_API.Models.DatabaseModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Restaurants_REST_API.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly int _refreshTokenLength;
        private readonly int _accessTokenValidityInDays;
        private readonly string _secretSignature;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _clientClaimName;
        private readonly string _employeeClaimName;

        public JwtService(IConfiguration config)
        {
            _config = config;
            
            _secretSignature = _config["ApplicationSettings:JwtSettings:SecretSignatureKey"];
            _issuer = _config["ApplicationSettings:JwtSettings:Issuer"];
            _audience = _config["ApplicationSettings:JwtSettings:Audience"];

            _clientClaimName = "ClientId";
            _employeeClaimName = "EmpId";

            try
            {
                _refreshTokenLength = int.Parse(_config["ApplicationSettings:JwtSettings:RefreshTokenLength"]);
                _accessTokenValidityInDays = int.Parse(_config["ApplicationSettings:JwtSettings:AccessTokenValidity"]);

                if (string.IsNullOrEmpty(_secretSignature))
                {
                    throw new Exception("Secret signature key of server can't be empty.");
                }

                if (string.IsNullOrEmpty(_issuer))
                {
                    throw new Exception("Issuer of jwt can't be empty.");
                }

                if (string.IsNullOrEmpty(_audience))
                {
                    throw new Exception("Audience for jwt can't be empty.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method generate refresh token. Length of token depends on given length in appsettings.json 
        /// </summary>
        /// <returns>Refresh token.</returns>
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

        /// <summary>
        /// Method generate access token for user. Token contains user claims and token is signed by server.
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>Signed access token.</returns>
        public string GenerateAccessTokenForUser(User user)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretSignature));

            SigningCredentials serverCreditionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var identity = new ClaimsIdentity(new[] 
            { 
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.UserRole),
            });

            string? empId = user.IdEmployee.ToString();
            string? clientId = user.IdClient.ToString();

            if (!string.IsNullOrEmpty(empId))
            {
                identity.AddClaim(new Claim(_employeeClaimName, empId));
            }

            if (!string.IsNullOrEmpty(clientId))
            {
                identity.AddClaim(new Claim(_clientClaimName, clientId));
            }

            var token = jwtHandler.CreateJwtSecurityToken
                (
                    subject: identity,
                    signingCredentials: serverCreditionals,
                    audience: _audience,
                    issuer: _issuer,
                    expires: DateTime.UtcNow.AddDays(_accessTokenValidityInDays)
                );
            return jwtHandler.WriteToken(token);
        }

        /// <summary>
        /// Method valids passed pair of access and refresh token.
        /// </summary>
        /// <param name="jwt">Pair of access token and refresh token.</param>
        /// <returns>True if token is valid or false  when token isn't valid.</returns>
        public bool ValidateJwt(PostJwtDTO jwt)
        {
            SecurityToken validatedToken;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretSignature))
            };

            try
            {
                var claim = new JwtSecurityTokenHandler().ValidateToken(jwt.AccessToken, parameters, out validatedToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool ValidateClientClaims(ClaimsIdentity? clientIdentity, int clientId)
        {
            if (clientIdentity == null)
            {
                return false;
            }

            string? jwtClientId = clientIdentity?.FindFirst(_clientClaimName)?.Value;
            if (jwtClientId == null)
            {
                return false;
            }

            if (clientId != int.Parse(jwtClientId))
            {
                return false;
            }

            return true;
        }
    }
}
