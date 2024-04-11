using Blog.Data;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;
[ApiController] [Route("v1")]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    
    // Injeção de dependência. AccountController depende de tokenService para existir
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
    {
        var users = await context.Users.ToListAsync();
        if (users != null)
        {
            try
            {
                return Ok(new ResultViewModel<List<User>>(users));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<User>("0x600 - Internal server Error.")); 
            }
        }
        return NotFound(new ResultViewModel<List<User>>("0x400 - No one user found."));
    }

    [HttpGet("accounts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return NotFound(new ResultViewModel<User>("0x401 - User not found."));
        try
        {
            return Ok(new ResultViewModel<User>(user));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("0x601 - Internal Server Error"));
        }
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> PostAsync(
        [FromServices] BlogDataContext context,
        [FromBody] AccountViewModel model)
    {
        if (!ModelState.IsValid) 
            return BadRequest();
        var user = new User(0, model.Name, model.Email, "hash", "ImagePath", model.Slug,model.Bio);
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Created($"accounts/{user.Id}", new ResultViewModel<User>(user));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("0x602 - Internal Server Error."));
        }

    }

    [HttpPut("accounts/{id:int}")]
    public async Task<IActionResult> PutAsync([FromServices] BlogDataContext context, [FromBody] AccountViewModel model, [FromRoute] int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return NotFound(new ResultViewModel<List<User>>("0x400 - No one user found."));
        try
        {
            user.Name = model.Name;
            user.Bio = model.Bio;
            user.Email = model.Email;
            try
            {
                user.Slug = model.Slug;
            }
            catch (ArgumentException e)
            {
                return StatusCode(500, new ResultViewModel<User>("0x301 - The statement 'model' are unique." ));
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<User>(user));
        }
        catch
        {
            return StatusCode(500,new ResultViewModel<User>( "0x602 - Internal Server Error."));
        }
    }
    
    [HttpDelete("accounts/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
            return NotFound();
        try
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<User>(user));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("0x603 - Internal Server Error."));
        }
    }

    [AllowAnonymous] // PERMITE Q O USUÁRIO ACESSE ESSE MÉTODO SEM ESTAR AUTORIZADO OU AUTENTICADO
    [HttpPost("login")]
    public IActionResult Login()
    {
        var token = _tokenService.GenerateToken(null);
        return Ok(token);
    }

    [Authorize(Roles = "user")] // BLOQUEIA O ACESSO DO CONTROLLER A TODO USUÁRIO QUE NÃO ESTEJA AUTORIZADO
    [HttpGet("user")]
    public IActionResult GetUser()
    {
        return Ok(User.Identity.Name);
    }


    [Authorize(Roles = "author")] // BLOQUEIA O ACESSO DO CONTROLLER A TODO USUÁRIO QUE NÃO ESTEJA AUTORIZADO
    [HttpGet("author")]
    public IActionResult GetAuthor()
    {
        return Ok(User.Identity.Name);
    }

    [Authorize(Roles = "admin")] // BLOQUEIA O ACESSO DO CONTROLLER A TODO USUÁRIO QUE NÃO ESTEJA AUTORIZADO
    [HttpGet("admin")]
    public IActionResult GetAdmin()
    {
        return Ok(User.Identity.Name);
    }
}