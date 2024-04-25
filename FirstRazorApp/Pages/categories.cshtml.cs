using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstRazorApp.Pages;

public class categories : PageModel
{
    public List<Item> x { get; set; } = new();
    public List<Item> Items { get; set; } = new();
    public record Item(int Id, 
        string Name, 
        decimal Price);
    
    public async Task OnGet(int skip= 0, int take=25)
    {
        await Task.Delay(1500);
        for (int i = 0; i < 100; i++)
        {   
            Items.Add(new Item(i,$"item {i}", i*12.132M));
        }

        
        x =Items.Skip(skip).Take(take).ToList();
    }
}