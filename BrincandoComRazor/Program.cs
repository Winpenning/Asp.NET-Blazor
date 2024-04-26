var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); // Adiciona suporte as páginas razor

var app = builder.Build();
app.UseStaticFiles(); // Habilita o uso de arquivos estáticos

app.Run();
