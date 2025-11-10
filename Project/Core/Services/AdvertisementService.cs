using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Data.Interfaces;
using BlocketChallenge.Project.Domain.DTOs;
using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Core.Services;

public class AdvertisementService(IAdvertisementRepository repository) : IAdvertisementService
{
    private readonly IAdvertisementRepository _repository = repository;

    public IEnumerable<AdvertisementDTO> GetAllAdvertisements(string? search, string? sort, string? order)
{
    var ads = _repository.GetAll();

    // 🔍 Optional search
    if (!string.IsNullOrWhiteSpace(search))
    {
        ads = ads.Where(ad =>
            ad.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            ad.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            (ad.Category.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
        );
    }

    // 🧭 Optional sorting
    ads = (sort?.ToLower(), order?.ToLower()) switch
    {
        ("price", "asc") => ads.OrderBy(a => a.Price),
        ("price", "desc") => ads.OrderByDescending(a => a.Price),
        ("date", "asc") => ads.OrderBy(a => a.CreatedAt),
        ("date", "desc") => ads.OrderByDescending(a => a.CreatedAt),
        _ => ads.OrderByDescending(a => a.CreatedAt) // Default sort by newest
    };

    // 🎯 Map to DTOs *after* filtering/sorting
    var dtos = ads.Select(ad => new AdvertisementDTO
    {
        Id = ad.Id,
        Title = ad.Title,
        Description = ad.Description,
        Price = ad.Price,
        ImageUrl = ad.ImageUrl,
        SellerName = ad.Seller.Username,
        SellerId = ad.Seller.Id,
        CategoryName = ad.Category.Name ,
        CreatedAt = ad.CreatedAt,
    });

    return dtos;
}

    public AdvertisementDTO? GetAdvertisementById(int id)
    {
        var ad = _repository.GetAdvertisementById(id);
        if (ad == null) return null;

        return new AdvertisementDTO
        {
            Id = ad.Id,
            Title = ad.Title,
            Description = ad.Description,
            Price = ad.Price,
            SellerName = ad.Seller.Username,
            SellerId = ad.Seller.Id,
            CategoryName = ad.Category.Name,
            CreatedAt = ad.CreatedAt,
            ImageUrl = ad.ImageUrl
        };
    }

    public void Create(Advertisement ad)
    {
        ad.CreatedAt = DateTime.UtcNow;
        if (string.IsNullOrWhiteSpace(ad.Title))
            throw new ArgumentException(ad.Title);

        if (ad.Price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        _repository.Create(ad);
    }

    public void Update(Advertisement ad)
    {
        if (ad.Id <= 0)
            throw new ArgumentException("Invalid Advertisement ID.");
        _repository.Update(ad);
    }

    public void Delete(int id)
    {
        _repository.Delete(id);
    }

    public IEnumerable<AdvertisementDTO> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return Enumerable.Empty<AdvertisementDTO>();

        var results = _repository.GetAll()
            .Where(ad =>
                ad.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (ad.Description != null && ad.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (ad.Category != null && ad.Category.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                (ad.Seller != null && ad.Seller.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            );

        // Map to DTOs
        return results.Select(ad => new AdvertisementDTO
        {
            Id = ad.Id,
            Title = ad.Title,
            Description = ad.Description,
            Price = ad.Price,
            ImageUrl = ad.ImageUrl,
            CategoryName = ad.Category?.Name ?? "Unknown",
            SellerName = ad.Seller?.Username ?? "Unknown",
            CreatedAt = ad.CreatedAt
        }).ToList();
    }

    public IEnumerable<Advertisement> GetByCategory(int categoryId)
    {
        return _repository.GetAll().Where(ad => ad.CategoryId == categoryId);
    }

    public IEnumerable<Advertisement> GetByUserId(int sellerId)
    {
        return _repository.GetByUserId(sellerId);
    }
}