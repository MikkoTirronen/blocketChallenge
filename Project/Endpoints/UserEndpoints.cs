using BlocketChallenge.Models;
using BlocketChallenge.Services;

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

        group.MapPost("/", (User user, IUserService service) =>
        {
            try
            {
                service.CreateUser(user);
                return Results.Created($"/api/user/user.Id", user);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
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

        group.MapGet("username/{username}", (string username, IUserService service) =>
        {
            var user = service.GetByUsername(username);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });
    }
}