using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class TokenService: ITokenService
    {
        private readonly JwtSettingsDTO _jwtSettings;
        public TokenService(IOptions<JwtSettingsDTO> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public TokenDTO JwtToken(User user, string type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            Claim[] claims;
            DateTime? expires;
            if(type.ToLower() == "login")
            {
                claims =
                [
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role, user.Role),
                ];
                expires = DateTime.UtcNow.AddDays(4);
            }else
            {
                claims = new Claim[]
                {
                    new(ClaimTypes.Email, user.Email),
                };
                expires = DateTime.UtcNow.AddMinutes(2);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDTO { Token = tokenHandler.WriteToken(token) };
        }
        public string ExtractUserDetials(string jwtToken, string type)
        {
            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(jwtToken);

            var claims = token.Claims;
            if (type.ToLower() == "email")
            {
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email");
                return userClaim?.Value ?? string.Empty;
            }
            else if (type.ToLower() == "username")
            {
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name");
                return userClaim?.Value ?? string.Empty;
            }
            else if (type.ToLower() == "role")
            {
                var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");
                return userClaim?.Value ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool ValidateToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var ValidateParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var result = tokenHandler.ValidateToken(jwtToken, ValidateParameters, out var ValidateToken);
                Console.WriteLine(result.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
