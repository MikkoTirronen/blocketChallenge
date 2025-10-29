using BlocketChallenge.Models;

namespace BlocketChallenge.Repositories;

public interface IAdvertisementRepository
{
    IEnumerable<Advertisement> GetAll();
    Advertisement GetAdvertisementById(int id);
    void Create();
    void Update();
    void Delete();
}