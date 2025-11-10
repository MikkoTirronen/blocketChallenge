using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Data.Interfaces;
public interface ICategoryRepository
{
    IEnumerable<Category> GetAll();
}