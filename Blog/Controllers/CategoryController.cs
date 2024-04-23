using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers;
[ApiController] [Route("v1")]
public class CategoryController : ControllerBase
{
    public List<Category> GetCategories([FromServices] BlogDataContext context)
    {
        var categories = context.Categories.ToList();
        return (categories);
    }
    [HttpGet("categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices]BlogDataContext context,
        [FromServices] IMemoryCache cache)
    {
        var categories = cache.GetOrCreate("CategoriesCache", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return GetCategories(context);
        });
        if (categories != null)
        {
            try 
            { return Ok(new ResultViewModel<List<Category>>(categories)); }
            
            catch 
            { return StatusCode(500, new ResultViewModel<List<Category>>("0x500 - Internal Server Error.")); }
        } 
        return NotFound();
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute]int id,[FromServices] BlogDataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound();
        try
        { return Ok(new ResultViewModel<Category>(category)); }
        catch (Exception e)
        { return StatusCode(500, new ResultViewModel<List<Category>>("0x501 - Internal Server Error.")); }
    }

    [HttpPost("categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] CategoryViewModel model,// Aplicar este quando conseguirmos puxar os dados do htmL
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var category = new Category(0, model.Name, model.Slug);
        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Created($"   categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id,[FromBody] CategoryViewModel model, [FromServices] BlogDataContext context)
    {
        var category =  await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound();
        category.Name = model.Name;
        category.Slug = model.Slug;
        context.Categories.Update(category);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Category>(category));
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        var category =  await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound();
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Category>(category));
    }
}