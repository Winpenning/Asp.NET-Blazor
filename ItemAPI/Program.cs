using ItemApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDataContext>(options => {
    options.UseSqlServer(connectionString);
});
var app = builder.Build();
app.UseCors(builder=>{
    builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();

});


app.MapControllers();

app.Run();

