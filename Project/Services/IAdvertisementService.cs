using BlocketChallenge.Models;
using BlocketChallenge.Services.DTOs;

namespace BlocketChallenge.Services;

public interface IAdvertisementService
{
    IEnumerable<AdvertisementDTO> GetAllAdvertisements();
    AdvertisementDTO? GetAdvertisementById(int id);

    void Create(Advertisement ad);
    void Update(Advertisement ad);
    void Delete(int id);

    IEnumerable<AdvertisementDTO> Search(string keyword);
    IEnumerable<Advertisement> GetByCategory(int categoryId);
    IEnumerable<Advertisement> GetByUserId(int sellerId);
}
