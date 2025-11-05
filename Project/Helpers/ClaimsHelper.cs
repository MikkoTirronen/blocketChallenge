using System.Security.Claims;

namespace BlocketChallenge.Helpers;

public static class ClaimsHelper
{
    public static int? GetUserID(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst("userId")?.Value;
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}