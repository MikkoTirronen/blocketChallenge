using BlocketChallenge.Models;
using BlocketChallenge.Services;
using Microsoft.AspNetCore.Identity.Data;

namespace BlocketChallenge.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {


        var group = routes.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", (IUserService service) => Results.Ok(service.GetAllUsers()));

        group.MapGet("/{id:int}", (int id, IUserService service) =>
        {
            var user = service.GetUserById(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });

        group.MapPost("/register", (User user, IUserService service) =>
        {
            try
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                user.CreatedAt = DateTime.UtcNow;

                service.CreateUser(user);
                return Results.Created($"/api/user/user.Id", new { user.Username, user.Email });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPost("/login", (LoginRequest request, IUserService service, IJwtTokenService jwt) =>
        {
            var user = service.Authenticate(request.Username, request.Password);
            if (user is null)
                return Results.Unauthorized();

            var token = jwt.GenerateToken(user);

            return Results.Ok(new
            {
                Message = "Login successful",
                Token = token,
                user.Username,
                user.Email
            });
        });

        group.MapPut("/{id:int}", (int id, User user, IUserService service) =>
        {
            if (id != user.Id)
                return Results.BadRequest("ID mismatch.");

            service.UpdateUser(user);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IUserService service) =>
        {
            service.DeleteUser(id);
            return Results.NoContent();
        });


    }
}
public record LoginRequest(string Username, string Password);