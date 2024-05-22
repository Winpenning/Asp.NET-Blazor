using ItemApi.Data;
using ItemApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItemAPI.Controllers;

[ApiController] [Route("items")]
public class ItemControler : ControllerBase
{
    [HttpGet][Route("")]
    public async Task<IActionResult> GetAsync
    ([FromServices] AppDataContext context) //retornar todos os item que temos 
    {
        List<Item> data = await context.Items.AsNoTracking().ToListAsync();
        return StatusCode(200, data);
    }

    [HttpPost][Route("")]
    public async Task<IActionResult> PostAynsc
    ([FromServices] AppDataContext context, [FromBody] Item item)
    {
        Item i = new Item(item.Name);
        
        try{
            context.Items.Add(i);
            await context.SaveChangesAsync();
            return StatusCode(200,item);
        }
        catch(Exception e){
            System.Console.WriteLine(e);
            return StatusCode(500,"MEU DEUS, O QUE EU FIZ DE ERRADO????");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync
    ([FromServices]  AppDataContext context, [FromRoute] int id){
        Item  item = new Item();
        item = await context.Items.AsNoTracking().FirstOrDefaultAsync(x=> x.Id == id);
        return StatusCode(200,item);
    }

    [HttpPut][Route("{id:int}")]
    public async Task<IActionResult> PutAsync
    ([FromServices] AppDataContext context, [FromRoute] int id, [FromBody] Item item)
    {
         Item updatedItem = await context.Items.FirstOrDefaultAsync(x=> x.Id == id); 

        if(item is not null)
            updatedItem.Name = item.Name;
        else
            return StatusCode(500);

        try{
            context.Items.Update(updatedItem);
            await context.SaveChangesAsync();
            return Ok(updatedItem);
        }
        catch(Exception e){
            return StatusCode(500, e);
        }

    }

    [HttpDelete][Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync
    ([FromServices] AppDataContext context, [FromRoute] int id)
    {
        Item? deletedItem = await context.Items.FirstOrDefaultAsync(x=>x.Id==id);
        if(deletedItem is null)
            return NotFound();
        context.Items.Remove(deletedItem);
        await context.SaveChangesAsync();
        return Ok(deletedItem);
    }

}