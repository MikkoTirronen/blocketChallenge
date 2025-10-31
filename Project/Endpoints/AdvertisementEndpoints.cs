using BlocketChallenge.Models;
using BlocketChallenge.Services;

namespace BlocketChallenge.Endpoints;

public static class AdvertisementEndpoints
{
    public static void MapAdvertisementEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ads").WithTags("Advertisements");

        group.MapGet("/", (IAdvertisementService service) =>
        {
            var ads = service.GetAllAdvertisements();
            return Results.Ok(ads);
        });

        group.MapGet("/{id:int}", (int id, IAdvertisementService service) =>
        {
            var ad = service.GetAdvertisementById(id);
            return ad is not null ? Results.Ok(ad) : Results.NotFound();
        });

        group.MapPost("/", (Advertisement ad, IAdvertisementService service) =>
        {
            try
            {
                service.Create(ad);
                return Results.Created($"/api/ads/{ad.Id}", ad);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        group.MapPut("/{id:int}", (int id, Advertisement ad, IAdvertisementService service) =>
        {
            if (id != ad.Id)
                return Results.BadRequest("Id failure.");

            service.Update(ad);

            return Results.NoContent();
        });

        
    }
}