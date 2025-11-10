using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Data.Interfaces;
using BlocketChallenge.Project.Domain.Models;




public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Category> GetAll() => _repo.GetAll();
}