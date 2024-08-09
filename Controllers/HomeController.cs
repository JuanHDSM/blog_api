using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get(
            [FromServices] IConfiguration configuration
        )
        {
            var env = configuration.GetValue<string>("Env");
            return Ok(new
            {
                enviroment = env
            });
        }
    }
}