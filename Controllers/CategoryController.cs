using blog_api.Extensions;
using blog_api.ViewModels;
using blog_api.ViewModels.Categories;
using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_api.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            [FromServices] AppDbContext context)
        {
            try
            {
                var categories = await context.Categories.AsNoTracking().ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("01X01 - Falha interna no servidor"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            try
            {
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("01X02 - Falha interna no servidor"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices] AppDbContext context
        )
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Posts = [],
                    Name = model.Name,
                    Slug = model.Slug
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"/categories/{category.Id}", category);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "01X03 - Não foi possível cadastrar a categoria");
            }
            catch
            {

                return StatusCode(500, "01X04 - Falha interna no servidor");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices] AppDbContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] AppDbContext context
        )
        {
            try
            {
                var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "01X05 - Falha interna no servidor");
            }
            catch
            {
                return StatusCode(500, "01X06 - Falha interna no servidor");
            }

        }

    }
}