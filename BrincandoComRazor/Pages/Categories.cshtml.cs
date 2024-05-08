using Microsoft.AspNetCore.Mvc.RazorPages;
using BrincandoComRazor.Data;
using BrincandoComRazor.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrincandoComRazor.Pages;

public class Categories : PageModel
{
    public List<Category> categories=new();
    public async Task OnGet()
    {
        var context = new AppDbContext();
        categories = context.Categories.ToList();
    }
}