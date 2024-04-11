using Blog.Data;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
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
            return BadRequest(new ResultViewModel<string>("0x300 - Invalid Request"));
        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@","-").Replace(".","-")
        };
        // Gerar a senha
        var password = PasswordGenerator.Generate(25, true, true);
        // Gera o hash da senha encriptando-a para salvar no banco
        user.PasswordHash = PasswordHasher.Hash(password);
        // GERAR SENHA FORTE PARA O USUÁRIO
        //Armazenamento das Senhas -> Encriptação
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email, password
            }));
        }
        catch(DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("0x620 - Email já cadastrado."));
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
            user.Email = model.Email;
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
    // MÉTODO DE LOGIN
    [AllowAnonymous] // PERMITE Q O USUÁRIO ACESSE ESSE MÉTODO SEM ESTAR AUTORIZADO OU AUTENTICADO
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] BlogDataContext context, [FromServices] TokenService tokenService)
    {
        // VERIFICA A VALIDADE DO MODEL
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>("0x301 - Invalid Request"));
        // RETORNA O USUÁRIO 
        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles) // devemos pegar os roles para fazer os Claims do tokens
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválida."));
        // VERIFICA VIA HASH (NÃO VIA STRING), OS HASH'S
        if(!PasswordHasher.Verify(user.PasswordHash,model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválida."));
        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("0x603 - Internal Server Error."));
        }
    }
    // [Authorize(Roles = "user")] // BLOQUEIA O ACESSO DO CONTROLLER A TODO USUÁRIO QUE NÃO ESTEJA AUTORIZADO
    // [HttpGet("user")]
    // public IActionResult GetUser()
    // {
    //     return Ok(User.Identity.Name);
    // }
    //
    //
    // [Authorize(Roles = "author")] // BLOQUEIA O ACESSO DO CONTROLLER A TODO USUÁRIO QUE NÃO ESTEJA AUTORIZADO
    // [HttpGet("author")]
    // public IActionResult GetAuthor()
    // {
    //     return Ok(User.Identity.Name);
    // }
    //
    // [Authorize(Roles = "admin")] // BLOQUEIA O ACESSO DO CONTROLLER A TODO USUÁRIO QUE NÃO ESTEJA AUTORIZADO
    // [HttpGet("admin")]
    // public IActionResult GetAdmin()
    // {
    //     return Ok(User.Identity.Name);
    // }
}