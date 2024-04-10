using System.Text;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

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



builder.Services.AddControllers().ConfigureApiBehaviorOptions
(opt => {opt.SuppressModelStateInvalidFilter = true; });

builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenService>(); // Permite com que a autenticação fique ativa por requisição única
//builder.Services.AddScoped(); //permite que a atutenticação dure por transação
//builder.Services.AddSingleton(); // implementa o padrão singleton
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
