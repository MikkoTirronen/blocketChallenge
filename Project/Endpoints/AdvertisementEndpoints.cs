using BlocketChallenge.Models;
using BlocketChallenge.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BlocketChallenge.Helpers;
namespace BlocketChallenge.Endpoints;

public static class AdvertisementEndpoints
{
    public static void MapAdvertisementEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ads").WithTags("Advertisements");


        //GetAll Advertisements
        group.MapGet("/", (IAdvertisementService service) =>
        {
            var ads = service.GetAllAdvertisements();
            return Results.Ok(ads);
        });

        //GetById Endpoint
        group.MapGet("/{id:int}", (int id, IAdvertisementService service) =>
        {
            var ad = service.GetAdvertisementById(id);
            return ad is not null ? Results.Ok(ad) : Results.NotFound();
        });

        //Protected: Create Endpoint
        group.MapPost("/", [Authorize] (Advertisement ad, IAdvertisementService service, ClaimsPrincipal user) =>
        {
            try
            {
                var userId = user.GetUserID();
                if (userId is null) return Results.Unauthorized();

                service.Create(ad);
                return Results.Created($"/api/ads/{ad.Id}", ad);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        //Protected: Update Endpoint
        group.MapPut("/{id:int}", [Authorize] (int id, Advertisement UpdatedAd, IAdvertisementService service, ClaimsPrincipal user) =>
        {
            var userId = user.GetUserID();
            if (userId is null) return Results.Unauthorized();

            if (id != UpdatedAd.Id)
                return Results.BadRequest("Advertisement Id doesnt match.");

            var existingAd = service.GetAdvertisementById(id);
            if (existingAd is null) return Results.NotFound();

            if (existingAd.SellerId != userId.Value)
                return Results.Forbid();

            service.Update(UpdatedAd);

            return Results.NoContent();
        });

        //Protected :Delete Endpoint
        group.MapDelete("/{id:int}", [Authorize] (int id, IAdvertisementService service, ClaimsPrincipal user) =>
        {

            var userId = user.GetUserID();
            if (userId is null) return Results.Unauthorized();

            var ad = service.GetAdvertisementById(id);
            if (ad is null) return Results.NotFound();

            
            if (ad.SellerId != userId.Value)
                return Results.Forbid();

            service.Delete(id);
            return Results.NoContent();
        });

        //Search Endpoint
        group.MapGet("/search/{keyword}", (string keyword, IAdvertisementService service) =>
        {
            var results = service.Search(keyword);

            return Results.Ok(results);
        });

        //GetCategoryById
        group.MapGet("/category/{categoryId:int}", (int categoryId, IAdvertisementService service) =>
        {
            var results = service.GetByCategory(categoryId);
            return Results.Ok(results);
        });
    }
}