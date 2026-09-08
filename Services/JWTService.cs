using Microsoft.IdentityModel.Tokens;
using ProjetoMultidiciplinar.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoMultidiciplinar.Services
{
    public class JwtService
    {
        public string GenerateToken(UsersModel user)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.ID.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    user.Name
                ),

                new Claim(
                    ClaimTypes.Email,
                    user.Email
                )
            };

            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new Exception("JWT_KEY não foi configurada.");
            }

            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new Exception("JWT_ISSUER não foi configurada.");
            }

            if (string.IsNullOrWhiteSpace(jwtAudience))
            {
                throw new Exception("JWT_AUDIENCE não foi configurada.");
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}