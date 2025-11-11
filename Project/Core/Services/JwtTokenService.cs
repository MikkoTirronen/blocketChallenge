using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Services;

public class JwtTokenService(IConfiguration config) : IJwtTokenService
{
    private readonly IConfiguration _config = config;

    public string GenerateAccessToken(User user)
    {
        // 1️⃣ Configuration values with proper null checks
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing in configuration.");
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"]; // ✅ fix: key name was lowercase 'audience'

        // 2️⃣ Use safer config parsing for expiry
        var expiresInMinutes = double.TryParse(_config["Jwt:ExpiresInMinutes"], out var minutes) ? minutes : 60;
        var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        // 3️⃣ Create signing credentials
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 4️⃣ Add meaningful claims
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        // 5️⃣ Generate token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken(User user)
    {
        var key = _config["jwt:RefreshKey"];
        var expires = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshExpiresDays"]!));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateRefreshToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshKey"]!));
        var handler = new JwtSecurityTokenHandler();

        try
        {
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            }, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}


