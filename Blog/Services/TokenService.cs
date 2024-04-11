using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Extensions;
using Blog.Models;
using Microsoft.IdentityModel.Tokens;
namespace Blog.Services;
public class TokenService
{
    public string GenerateToken(User user)
    {
        // manipulador de tokens jwt
        var tokenHandler = new JwtSecurityTokenHandler();

        // Transforma o hash do token em um array de bytes
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

        var claims = user.GetClaims();
        
        
        // Configura o token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // "Assunto"
            Subject = new ClaimsIdentity(claims),
            // Duração do token
            Expires = DateTime.UtcNow.AddHours(2),
            // credenciais de autenticação e algoritmo de criptografia.
            SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}