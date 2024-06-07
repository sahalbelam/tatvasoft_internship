using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = jwtSettings.GetValue<string>("Key") ?? throw new ArgumentNullException("JWT Key is not configured.");

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key), "JWT Key is not configured.");
        }

        var keyBytes = Encoding.ASCII.GetBytes(key);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("name", username) }),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpireMinutes")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings.GetValue<string>("Issuer"),
            Audience = jwtSettings.GetValue<string>("Audience")
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}