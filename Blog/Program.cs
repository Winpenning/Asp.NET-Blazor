using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens; 

/*
 * @author: Paulo Henrique Ziemer dos Santos (Winpenning)
 * Web API para fins de estudo
 */

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);
App(app);


void LoadConfiguration(WebApplication app)
{
    // uso da ApiKey
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("smtp").Bind(smtp); // pega uma seção de dados
//app.Configuration.GetValue -> retorna o valor 
    Configuration.Smtp = smtp;
}
void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x=>x.TokenValidationParameters= new TokenValidationParameters
    {
        ValidateIssuerSigningKey =true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    });
}
void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });
    builder.Services.AddControllers().ConfigureApiBehaviorOptions
        (opt => {opt.SuppressModelStateInvalidFilter = true; })
        .AddJsonOptions(X =>
        {
            // ignora os ciclos subsequentes de serialização de dados
            X.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
            // quando retornar um objeto nulo, ele não renderizará o objeto na tela
            X.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });
}
void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenService>(); // Permite com que a autenticação fique ativa por requisição única
    //builder.Services.AddScoped(); //permite que a atutenticação dure por transação
    //builder.Services.AddSingleton(); // implementa o padrão singleton
    builder.Services.AddTransient<EmailService>();
}
void App(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseResponseCompression();
    app.UseStaticFiles();
    app.MapControllers();
    app.Run();
}