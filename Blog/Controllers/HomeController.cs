using Blog.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;
[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get(IConfiguration config)
    {
        var env = config.GetValue<string>("Env");
        return Ok(new
        {
            environment = env
        });
    }
}