using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Blog.Controllers;
[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("categories")]
    public async Task<IActionResult> GetAsync([FromServices]BlogDataContext context)
    {
        var categories = await context.Categories.ToListAsync();
        if (categories != null)
        {
            try
            {
                return Ok(categories);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Get");
            }
        }
        else
        {
            return NotFound();
        }
        
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id,[FromServices] BlogDataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return StatusCode(404);
        
        try
        {
            return Ok(category);
        }
        catch (Exception e)
        {
            return StatusCode(500,"GetById");
        }
        
    }

    [HttpPost("categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] Category model,// Aplicar este quando conseguirmos puxar os dados do htmL
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug,
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500);
        }
    }
    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id,[FromBody] Category model, [FromServices] BlogDataContext context)
    {
        var category =  await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return StatusCode(404);
        
        category.Name = model.Name;
        category.Slug = model.Slug;
        
        context.Categories.Update(category);
        await context.SaveChangesAsync();
        return StatusCode(200, model);
    }
    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        var category =  await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return StatusCode(404);
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return StatusCode(200, category);
    }
}