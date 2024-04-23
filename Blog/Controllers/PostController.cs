using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;
[ApiController] [Route("posts")]
public class PostController : ControllerBase
{

    [HttpGet][Route("")]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context, [FromQuery]int page = 0, [FromQuery]int pageSize = 10)
    {
        var count = await context.Posts.AsNoTracking().CountAsync();
        List<ListPostViewModel> data = await context
            .Posts
            .AsNoTracking()
            .Include(x => x.Author)
            .Include(x => x.Category)
            .Select(x =>
                new ListPostViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpadteDate = x.LastUpdateDate,
                    Author = $"{x.Author.Name} ({x.Author.Email})",
                    Category = x.Category.Name
                })
            .Skip(page * pageSize)
            .Take(pageSize)
            .OrderByDescending(x => x.LastUpadteDate)
            .ToListAsync();
        return Ok(new ResultViewModel<dynamic>(new
        {
            total = count,
            page,
            pageSize,
            data
        }));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> DetailsAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var post = await context
                .Posts.AsNoTracking()
                .Include(x => x.Author)
                //podemos utilizar o ThenInclude para carregar listas que estão dentro de sub-documentos (porém gasta mais processamento, se puder evitar, melhor)
                .ThenInclude(x => x.Roles)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
                return NotFound();
            return Ok(new ResultViewModel<Post>(post));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("0x501 - Internal Server Error."));
        }
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategoryAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] string category,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 25)
    {
        try
        {
            var count = await context.Posts.AsNoTracking().CountAsync();
            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category.Slug == category)
                .Select(x => new ListPostViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpadteDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name} ({x.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpadteDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("0x502 - Internal Server Error"));
        }
    }
}