using BlocketChallenge.Models;
using BlocketChallenge.Repositories;
using BlocketChallenge.Services.DTOs;

namespace BlocketChallenge.Services;

public class AdvertisementService(IAdvertisementRepository repository) : IAdvertisementService
{
    private readonly IAdvertisementRepository _repository = repository;

    public IEnumerable<AdvertisementDTO> GetAllAdvertisements()
    {
        var ads = _repository.GetAll();
        return ads.Select(ad => new AdvertisementDTO
        {
            Id = ad.Id,
            Title = ad.Title,
            Description = ad.Description,
            Price = ad.Price,
            SellerName = ad.Seller.Username,
            CategoryName = ad.Category.Name,
            CreatedAt = ad.CreatedAt,
            ImageUrl = ad.ImageUrl
        });
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

    public IEnumerable<Advertisement> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return [];

        return _repository.GetAll()
            .Where(ad => ad.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || ad.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Advertisement> GetByCategory(int categoryId)
    {
        return _repository.GetAll().Where(ad => ad.CategoryId == categoryId);
    }


}