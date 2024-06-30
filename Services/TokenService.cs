using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BlogApi;
using BlogApi.Models;

namespace blog_api.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        }
    }
}