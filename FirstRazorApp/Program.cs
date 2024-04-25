var builder = WebApplication.CreateBuilder(args); // INICIALIZA UMA APLICAÇÃO WEB
builder.Services.AddRazorPages(); // ADICIONA SUPORTE AO RAZOR

var app = builder.Build(); // CRIA UMA INSTÂNCIA DO SERVIDOR
app.UseHttpsRedirection();
app.UseStaticFiles(); // Arquivos servidos no wwwroot
app.UseRouting();
app.MapRazorPages();

app.Run(); // EXECUTA A APLICAÇÃO
