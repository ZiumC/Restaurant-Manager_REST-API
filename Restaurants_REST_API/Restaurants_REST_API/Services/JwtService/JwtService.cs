﻿using Microsoft.IdentityModel.Tokens;
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

        public JwtService(IConfiguration config)
        {
            _config = config;
            try
            {
                _refreshTokenLength = int.Parse(_config["ApplicationSettings:JwtSettings:RefreshTokenLength"]);
                _accessTokenValidityInDays = int.Parse(_config["ApplicationSettings:JwtSettings:AccessTokenValidity"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            _secretSignature = _config["ApplicationSettings:JwtSettings:SecretSignatureKey"];
            _issuer = _config["ApplicationSettings:JwtSettings:Issuer"];
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

        public string GenerateAccessTokenForUserLogin(string userLogin, string userRole)
        {
            var userClaims = new Claim[] 
            {
                    new Claim(ClaimTypes.Name, userLogin),
                    new Claim(ClaimTypes.Role, userRole)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretSignature));

            SigningCredentials serverCreditionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    _issuer,
                    _issuer,
                    userClaims,
                    expires: DateTime.UtcNow.AddDays(_accessTokenValidityInDays),
                    signingCredentials: serverCreditionals
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
