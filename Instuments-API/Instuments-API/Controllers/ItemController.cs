using Instuments_API.Data;
using Instuments_API.Models;
using Instuments_API.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Type = Instuments_API.Models.Enumerations.Type;

namespace Instuments_API.Controllers;

[ApiController] [Route("item")]
public class ItemController : ControllerBase
{
     [HttpGet]
     public async Task<IActionResult> GetAsync([FromServices] InstrumentDataContext context)
     {
         var instruments = await context.items.ToListAsync();
         try
         {
             return Ok(instruments);
         }
         catch
         {
             return StatusCode(500, "0x500 - Internal Server Error.");
         }
     }

     [HttpPost]
     public async Task<IActionResult> PostAsync([FromServices] InstrumentDataContext context, [FromBody] ItemViewModel model)
     {
         Type t = Type.Instrument;
         if (model.Type == "Instrument")
             t = Type.Instrument;
         if (model.Type == "Acessory")
             t = Type.Acessory;
         if (model.Type == "SoundBox")
             t = Type.SoundBox;
         
             
         Item item = new Item
         {
             Id = 0,
             Brand = model.Brand,
             Type = t,
             Model = model.Model,
             USDPrice = model.USDPrice
         };

         try
         {
            await context.items.AddAsync(item);
             await context.SaveChangesAsync();
             return Created();
         }
         catch
         {
             return StatusCode(500);
         }
     }
}