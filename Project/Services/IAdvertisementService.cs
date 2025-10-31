using BlocketChallenge.Models;
namespace BlocketChallenge.Services;

public interface IAdvertisementService
{
    IEnumerable<Advertisement> GetAllAdvertisements();
    Advertisement? GetAdvertisementById(int id);

    void Create(Advertisement ad);
    void Update(Advertisement ad);
    void Delete(int id);

    IEnumerable<Advertisement> Search(string keyword);
    IEnumerable<Advertisement> GetByCategory(int categoryId);
}
