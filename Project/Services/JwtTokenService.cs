using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BlocketChallenge.Models;

namespace BlocketChallenge.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}

public class JwtTokenService(IConfiguration config) : IJwtTokenService
{
    private readonly IConfiguration _config = config;

    public string GenerateToken(User user)
    {
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt Key missing.");
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt: audience"];
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"] ?? "60"));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim (JwtRegisteredClaimNames.Sub, user.Username),
            new Claim (JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim ("userId", user.Id.ToString()),
            new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


