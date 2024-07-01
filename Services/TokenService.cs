using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BlogApi;
using BlogApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace blog_api.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
            var tokenDescripton = new SecurityTokenDescriptor
            {
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