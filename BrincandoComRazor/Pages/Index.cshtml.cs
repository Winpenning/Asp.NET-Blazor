using BrincandoComRazor.Data;
using BrincandoComRazor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BrincandoComRazor.Pages;

public class Index : PageModel
{
    public List<Post> cat { get; set; } = new();
    public async Task OnGet()
    {
        AppDbContext c= new AppDbContext();
        var item = await c.Posts.ToListAsync();
        cat = item;
    }
}