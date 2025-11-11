using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BlocketChallenge.Helpers;
using Serilog;
using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Domain.DTOs;
using BlocketChallenge.Project.Domain.Models;
namespace BlocketChallenge.Endpoints;

public class AdvertisementEndpointsLogger { }
public static class AdvertisementEndpoints
{
    public static void MapAdvertisementEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ads").WithTags("Advertisements");

        //GetAll Advertisements
        group.MapGet("/", (string? search, string? sort, string? order, IAdvertisementService service) =>
        {
            var result = service.GetAllAdvertisements(search, sort, order);
            return Results.Ok(result);
        });

        //GetById
        group.MapGet("/{id:int}", (int id, IAdvertisementService service) =>
        {
            var ad = service.GetAdvertisementById(id);
            return ad is not null ? Results.Ok(ad) : Results.NotFound();
        });

        //Create
        group.MapPost("/", [Authorize] (AdvertisementCreateDto dto, IAdvertisementService service, ClaimsPrincipal user) =>
        {
            try
            {

                foreach (var claim in user.Claims)
                {
                    Log.Information("Claim type: {ClaimType}, Claim value: {ClaimValue}", claim.Type, claim.Value);
                }


                var userIdClaim = user.GetUserID();
                if (userIdClaim == null)
                {
                    Log.Warning("No user ID found in claims.");
                    return Results.Unauthorized();
                }
                var ad = new Advertisement
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId,
                    SellerId = userIdClaim.Value,
                    ImageUrl = dto.ImageUrl,
                };
                Log.Information($"Creating ad with CategoryId={ad.CategoryId}, SellerId={ad.SellerId}", dto.CategoryId, userIdClaim, dto.ImageUrl);

                service.Create(ad);

                Log.Information("Advertisement created successfully: {@Ad}", ad);
                return Results.Created($"/api/ads/{ad.Id}", ad);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating advertisement");
                return Results.BadRequest(new { error = ex.Message });
            }
        });
        


        //Protected: Update Endpoint
        group.MapPut("/{id:int}", [Authorize] (int id, Advertisement UpdatedAd, IAdvertisementService service, ClaimsPrincipal user) =>
        {
            Log.Warning("Received PUT /api/ads/{Id} with body: {@UpdatedAd}", id, UpdatedAd);

            var userId = user.GetUserID();
            if (userId is null) return Results.Unauthorized();

            // if (id != UpdatedAd.Id)
            //     return Results.BadRequest("Advertisement Id doesnt match.");

            var existingAd = service.GetAdvertisementById(id);
            if (existingAd is null) return Results.NotFound();

            if (existingAd.SellerId != userId.Value)
                return Results.Forbid();
            UpdatedAd.Id = id;
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

        // GET /api/categories
        group.MapGet("/categories", (ICategoryService service) =>
        {
            var categories = service.GetAll();
            return Results.Ok(categories);
        });
        //GetCategoryById
        group.MapGet("/category/{categoryId:int}", (int categoryId, IAdvertisementService service) =>
        {
            var results = service.GetByCategory(categoryId);
            return Results.Ok(results);
        });

    

    }
}