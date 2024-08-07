using System.Text.RegularExpressions;
using blog_api.Extensions;
using blog_api.Services;
using blog_api.ViewModels;
using blog_api.ViewModels.Accounts;
using BlogApi.Data;
using BlogApi.Models;
using BlogApi.Services;
using BlogApi.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromBody] UploadImageViewModel model,
            [FromServices] AppDbContext context
        )
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,")
                .Replace(model.Base64Image, "");
            var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
            }
            catch (Exception)
            {
                StatusCode(500, "02X04 - Falha interna no servidor");
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

            if(user == null)
                return NotFound(new ResultViewModel<string>("Usuário não encontrado"));

            user.Image = $"https://localhost:0000/images/{fileName}";

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "02X05 - Falha interna no servidor");
            }

            return Ok(new ResultViewModel<string>("Imagem alterada com sucesso", null!));
        }
    }
}

