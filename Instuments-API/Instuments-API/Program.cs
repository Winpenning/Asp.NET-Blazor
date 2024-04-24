using Instuments_API.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InstrumentDataContext>();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();
