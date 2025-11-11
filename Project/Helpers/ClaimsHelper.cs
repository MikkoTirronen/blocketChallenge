using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlocketChallenge.Helpers;

public static class ClaimsHelper
{
    public static int? GetUserID(this ClaimsPrincipal User)
    {
        var IdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
             ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return int.TryParse(IdClaim, out var id) ? id : null;
    }
}