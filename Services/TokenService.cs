using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BlogApi;
using BlogApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using blog_api.Extensions;

namespace blog_api.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
            var claims = user.GetClaims();
            var tokenDescripton = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescripton);
            return tokenHandler.WriteToken(token);
        }
    }
}