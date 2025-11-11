
using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Domain.DTOs;
using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {


        var group = routes.MapGroup("/api").WithTags("Users");

        //group.MapGet("/", (IUserService service) => Results.Ok(service.GetAllUsers()));

        group.MapGet("/users/{username}", (string username, IAdvertisementService service) =>
        {
            var listings = service.GetListingsByUsername(username);

            // Return public fields only
            return Results.Ok(listings.Select(l => new
            {
                l.Id,
                l.Title,
                l.Description,
                l.Price,
                l.ImageUrl,
                l.CreatedAt
            }));
        });


        group.MapPost("/auth/register", (UserCreateDTO dto, IUserService service) =>
        {
            try
            {
                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    CreatedAt = DateTime.UtcNow
                };

                service.CreateUser(user);
                return Results.Created($"/api/user/{user.Id}", new { user.Username, user.Email });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPost("/auth/login", (LoginRequest request, IUserService service, IJwtTokenService jwt, HttpResponse response) =>
        {
            var user = service.Authenticate(request.Username, request.Password);
            if (user is null)
                return Results.Unauthorized();

            var accessToken = jwt.GenerateAccessToken(user);
            var refreshToken = jwt.GenerateRefreshToken(user);

            response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //change for HTTPS
                SameSite = SameSiteMode.None, //change for HTTPS
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/api/auth/refresh"
            });

            return Results.Ok(new
            {
                Message = "Login successful",
                Token = accessToken,
                user.Username,
                user.Email
            });
        });

        group.MapPost("/auth/refresh", (HttpRequest request, IJwtTokenService jwt, IUserService userService) =>
        {
            if (!request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Results.Unauthorized();
            var principal = jwt.ValidateRefreshToken(refreshToken);
            if (principal is null)
                return Results.Unauthorized();

            var userId = principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userId is null)
                return Results.Unauthorized();

            var user = userService.GetUserById(int.Parse(userId));
            if (user is null)
                return Results.Unauthorized();

            var newAccessToken = jwt.GenerateAccessToken(user);
            return Results.Ok(new { Toekn = newAccessToken });
        });

        group.MapPost("/auth/logout", (HttpResponse response) =>
        {
            response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //change for production HTTPS
                SameSite = SameSiteMode.None,// strict for production HTTPS
                Path = "/api/auth/refresh"
            });
        });

        // group.MapPut("/{id:int}", (int id, User user, IUserService service) =>
        // {
        //     if (id != user.Id)
        //         return Results.BadRequest("ID mismatch.");

        //     service.UpdateUser(user);
        //     return Results.NoContent();
        // });

        // group.MapDelete("/{id:int}", (int id, IUserService service) =>
        // {
        //     service.DeleteUser(id);
        //     return Results.NoContent();
        // });


    }
}
public record LoginRequest(string Username, string Password);