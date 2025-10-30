
using BlocketChallenge.ConnectionFactories;
using BlocketChallenge.Models;
using Microsoft.Data.SqlClient;

namespace BlocketChallenge.Repositories;

public class AdvertisementRepository(DbConnectionFactory connectionFactory) : BaseRepository(connectionFactory), IAdvertisementRepository
{
    public IEnumerable<Advertisement> GetAll()
    {
        var ads = new List<Advertisement>();
        using var reader = ExecuteReader("SELECT * FROM Advertisements");

        while (reader.Read())
        {
            ads.Add(new Advertisement
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                SellerId = reader.GetInt32(4),
                CategoryId = reader.GetInt32(5),
                CreatedAt = reader.GetDateTime(6),
                SellerUsername = reader.GetString(7),
                CategoryName = reader.GetString(8)
            });
        }
        return ads;
    }

    public Advertisement? GetAdvertisementById(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("SELECT * FROM Advertisements Where Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Advertisement
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                SellerId = reader.GetInt32(4),
                CategoryId = reader.GetInt32(5),
                CreatedAt = reader.GetDateTime(6),
                SellerUsername = reader.GetString(7),
                CategoryName = reader.GetString(8)
            };
        }
        return null;
    }

    public void Create(Advertisement ad)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand(
            "INSERT INTO Advertisements (Title, Description, Price, SellerId, CreatedAt, SellerUsername, Category) VALUES (@Title, @Description, @Price, @SellerId, @CreatedAt, @SellerUsername, @CategoryName)"
        );

        cmd.Parameters.AddWithValue("@Title", ad.Title);
        cmd.Parameters.AddWithValue("@Description", ad.Description);
        cmd.Parameters.AddWithValue("@Price", ad.Price);
        cmd.Parameters.AddWithValue("@SellerId", ad.SellerId);
        cmd.Parameters.AddWithValue("@CreatedAt", ad.CreatedAt);
        cmd.Parameters.AddWithValue("@SellerUsername", ad.SellerUsername);
        cmd.Parameters.AddWithValue("@Category", ad.CategoryName);
        cmd.ExecuteNonQuery();
    }

    public void Update(Advertisement ad)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("UPDATE Advertisements Set Title=@Title, Description = @Description, Price = @Price Where Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Title", ad.Title);
        cmd.Parameters.AddWithValue("@Description", ad.Description);
        cmd.Parameters.AddWithValue("@Price", ad.Price);
        cmd.Parameters.AddWithValue("@Id", ad.Id);

        cmd.ExecuteNonQuery();
    }
    
    public void Delete(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("DELETE FROM Advertisements WHERE Id = @Id");
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }
}