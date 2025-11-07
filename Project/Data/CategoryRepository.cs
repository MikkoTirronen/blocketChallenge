using BlocketChallenge.ConnectionFactories;
using BlocketChallenge.Models;
using Microsoft.Data.SqlClient;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAll();
}

public class CategoryRepository(DbConnectionFactory connectionFactory) : ICategoryRepository
{
    private readonly DbConnectionFactory _connectionFactory = connectionFactory;

    public IEnumerable<Category> GetAll()
    {
        var categories = new List<Category>();
        var sql = "SELECT Id, Name FROM Categories";
        using var conn = _connectionFactory.CreateConnection();

        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            categories.Add(new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return categories;
    }
}