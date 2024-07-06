using blog_api.Extensions;
using blog_api.Services;
using blog_api.ViewModels;
using BlogApi.Data;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AccountController : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Post(
            [FromBody] RegisterViewModel model,
            [FromServices] EmailService emailService,
            [FromServices] AppDbContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            };

            var password = PasswordGenerator.Generate(25);

            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                emailService.Send(
                    user.Name, 
                    user.Email, 
                    "Welcome to Holy.io", 
                    $"Your Password is <strong>{password}</strong>");

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("02X01 - Este E-mail já está cadastrado"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("02X02 - Falha interna no servidor"));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(
            [FromBody] LoginViewModel model,
            [FromServices] AppDbContext context,
            [FromServices] TokenService tokenService)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null!));
            }
            catch
            {
                return StatusCode(500, "02X03 - Falha interna no servidor");
            }
        }
    }
}