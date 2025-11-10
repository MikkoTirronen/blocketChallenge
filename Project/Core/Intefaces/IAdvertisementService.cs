using BlocketChallenge.Project.Domain.DTOs;
using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Core.Interfaces;

public interface IAdvertisementService
{
    IEnumerable<AdvertisementDTO> GetAllAdvertisements(string? search, string? sort, string? order);
    AdvertisementDTO? GetAdvertisementById(int id);

    void Create(Advertisement ad);
    void Update(Advertisement ad);
    void Delete(int id);

    IEnumerable<AdvertisementDTO> Search(string keyword);
    IEnumerable<Advertisement> GetByCategory(int categoryId);
    IEnumerable<Advertisement> GetByUserId(int sellerId);
}
