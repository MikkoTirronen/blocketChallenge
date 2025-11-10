using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Core.Interfaces;
public interface IJwtTokenService
{
    string GenerateToken(User user);
}