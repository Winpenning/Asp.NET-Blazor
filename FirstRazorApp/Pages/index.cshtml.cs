using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstRazorApp.Pages;

public class index : PageModel
{
    public List<Item> Items { get; set; } = new();
    public record Item(int Id, 
        string Name, 
        decimal Price);
    
    public async Task OnGet(int skip, int take)
    {
        await Task.Delay(1500);
        for (int i = 0; i < 100; i++)
        {   
            Items.Add(new Item(i,$"item {i}", i*12.132M));
        }

        Items.Skip(skip).Take(take).ToList();
    }
}







