using BlocketChallenge.ConnectionFactories;
using BlocketChallenge.Models;
using BlocketClallenge.Repositories;
using Microsoft.Data.SqlClient;

namespace BlocketChallenge.Repositories;

public class UserRepository(DbConnectionFactory connectionFactory) : BaseRepository(connectionFactory), IUserRepository
{
    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();

        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, Username,Email,PasswordHash, CreatedAt FROM Users", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                PasswordHash = reader.GetString(3),
                CreatedAt = reader.GetDateTime(4)
            });
        }
        return users;
    }

    public void CreateUser(User user)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("INSERT INTO USERS(Username, Email,PasswordHash, CreatedAt) VALUES (@Username, @Email, @PasswordHash, @CreatedAt)", conn);

        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

        cmd.ExecuteNonQuery();
    }

    public User? GetUserById(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, Username, Email, PasswordHash, CreatedAt From Users Where Id=@Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                PasswordHash = reader.GetString(3),
                CreatedAt = reader.GetDateTime(4)
            };
        }
        return null;
    }
    public void Update(User user)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand(
            "Update Users SET Username=@Username, Email=@Email, PasswordHash=@PasswordHash WHERE Id=@Id", conn
        );

        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@Id", user.Id);

        cmd.ExecuteNonQuery();
    }
    public void Delete(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("DELETE FROM Users WHERE ID=@Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public User? GetUserByUsername(string username)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, Username, Email, PasswordHash,CreatedAt FROM Users WHERE Username=@Username", conn);

        cmd.Parameters.AddWithValue("@Username", username);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                PasswordHash = reader.GetString(3),
                CreatedAt = reader.GetDateTime(4)
            };
        }
        return null;
    }
}