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
        return StatusCode(200, categories);
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id,[FromServices] BlogDataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return StatusCode(404);
        return StatusCode(200, category);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> PostAsync([FromBody] Category model, [FromServices] BlogDataContext context)
    {
        await context.Categories.AddAsync(model);
        await context.SaveChangesAsync();
        return Created($"categories/{model.Id}",model);
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