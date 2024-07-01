using blog_api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AccountController : ControllerBase
    {
        [HttpPost("/login")]
        public IActionResult login([FromServices] TokenService tokenService)
        {
            var token = tokenService.GenerateToken(null);
            return Ok(token);
        }
    }
}