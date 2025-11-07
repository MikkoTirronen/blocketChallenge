using BlocketChallenge.Models;

public interface ICategoryService
{
    IEnumerable<Category> GetAll();
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<Category> GetAll() => _repo.GetAll();
}