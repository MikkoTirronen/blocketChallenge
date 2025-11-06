using BlocketChallenge.Models;

namespace BlocketChallenge.Repositories;

public interface IAdvertisementRepository
{
    IEnumerable<Advertisement> GetAll();
    Advertisement? GetAdvertisementById(int id);
    void Create(Advertisement ad);
    void Update(Advertisement ad);
    void Delete(int id);

    IEnumerable<Advertisement> GetByUserId(int sellerId);
}