using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Data.Interfaces;

public interface IAdvertisementRepository
{
    IEnumerable<Advertisement> GetAll();
    Advertisement? GetAdvertisementById(int id);
    void Create(Advertisement ad);
    void Update(Advertisement ad);
    void Delete(int id);

    IEnumerable<Advertisement> GetByUserId(int sellerId);
}