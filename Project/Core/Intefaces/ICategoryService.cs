using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Core.Interfaces;
public interface ICategoryService
{
    IEnumerable<Category> GetAll();
}