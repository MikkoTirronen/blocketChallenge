using System.Security.Claims;
using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Core.Interfaces;
public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
    ClaimsPrincipal? ValidateRefreshToken(string token);
}