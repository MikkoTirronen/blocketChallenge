using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BlocketChallenge.ConnectionFactories;
using BlocketChallenge.Models;

namespace BlocketChallenge.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AdvertisementRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Advertisement> GetAll()
        {
            var ads = new List<Advertisement>();

            const string sql = @"
                SELECT 
                    a.Id       AS AdId,
                    a.Title,
                    a.Description,
                    a.Price,
                    a.SellerId AS AdSellerId,
                    a.CategoryId AS AdCategoryId,
                    a.CreatedAt AS AdCreatedAt,
                    u.Id       AS UserId,
                    u.Username AS UserUsername,
                    u.Email    AS UserEmail,
                    u.CreatedAt AS UserCreatedAt,
                    c.Id       AS CategoryId,
                    c.Name     AS CategoryName
                FROM Advertisements a
                JOIN Users u ON a.SellerId = u.Id
                JOIN Categories c ON a.CategoryId = c.Id
                ORDER BY a.CreatedAt DESC;";

            using var conn = _connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            // get ordinals once
            var adIdOrd = reader.GetOrdinal("AdId");
            var titleOrd = reader.GetOrdinal("Title");
            var descOrd = reader.GetOrdinal("Description");
            var priceOrd = reader.GetOrdinal("Price");
            var adSellerIdOrd = reader.GetOrdinal("AdSellerId");
            var adCategoryIdOrd = reader.GetOrdinal("AdCategoryId");
            var adCreatedAtOrd = reader.GetOrdinal("AdCreatedAt");

            var userIdOrd = reader.GetOrdinal("UserId");
            var userUsernameOrd = reader.GetOrdinal("UserUsername");
            var userEmailOrd = reader.GetOrdinal("UserEmail");
            var userCreatedAtOrd = reader.GetOrdinal("UserCreatedAt");

            var categoryIdOrd = reader.GetOrdinal("CategoryId");
            var categoryNameOrd = reader.GetOrdinal("CategoryName");

            while (reader.Read())
            {
                var ad = new Advertisement
                {
                    Id = reader.GetInt32(adIdOrd),
                    Title = reader.GetString(titleOrd),
                    Description = reader.IsDBNull(descOrd) ? null : reader.GetString(descOrd),
                    Price = reader.GetDecimal(priceOrd),
                    SellerId = reader.GetInt32(adSellerIdOrd),
                    CategoryId = reader.GetInt32(adCategoryIdOrd),
                    CreatedAt = reader.GetDateTime(adCreatedAtOrd),

                    Seller = new User
                    {
                        Id = reader.GetInt32(userIdOrd),
                        Username = reader.GetString(userUsernameOrd),
                        Email = reader.IsDBNull(userEmailOrd) ? null : reader.GetString(userEmailOrd),
                        CreatedAt = reader.GetDateTime(userCreatedAtOrd)
                    },

                    Category = new Category
                    {
                        Id = reader.GetInt32(categoryIdOrd),
                        Name = reader.GetString(categoryNameOrd)
                    }
                };

                ads.Add(ad);
            }

            return ads;
        }

        public Advertisement? GetAdvertisementById(int id)
        {
            const string sql = @"
                SELECT 
                    a.Id       AS AdId,
                    a.Title,
                    a.Description,
                    a.Price,
                    a.SellerId AS AdSellerId,
                    a.CategoryId AS AdCategoryId,
                    a.CreatedAt AS AdCreatedAt,
                    u.Id       AS UserId,
                    u.Username AS UserUsername,
                    u.Email    AS UserEmail,
                    u.CreatedAt AS UserCreatedAt,
                    c.Id       AS CategoryId,
                    c.Name     AS CategoryName
                FROM Advertisements a
                JOIN Users u ON a.SellerId = u.Id
                JOIN Categories c ON a.CategoryId = c.Id
                WHERE a.Id = @Id;";

            using var conn = _connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            var ad = new Advertisement
            {
                Id = reader.GetInt32(reader.GetOrdinal("AdId")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                SellerId = reader.GetInt32(reader.GetOrdinal("AdSellerId")),
                CategoryId = reader.GetInt32(reader.GetOrdinal("AdCategoryId")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("AdCreatedAt")),
                Seller = new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("UserUsername")),
                    Email = reader.IsDBNull(reader.GetOrdinal("UserEmail")) ? null : reader.GetString(reader.GetOrdinal("UserEmail")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("UserCreatedAt"))
                },
                Category = new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                }
            };

            return ad;
        }

        public void Create(Advertisement ad)
        {
            const string sql = @"
                INSERT INTO Advertisements (Title, Description, Price, SellerId, CategoryId, CreatedAt)
                VALUES (@Title, @Description, @Price, @SellerId, @CategoryId, @CreatedAt);";

            using var conn = _connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Title", ad.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", ad.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", ad.Price);
            cmd.Parameters.AddWithValue("@SellerId", ad.SellerId);
            cmd.Parameters.AddWithValue("@CategoryId", ad.CategoryId);
            cmd.Parameters.AddWithValue("@CreatedAt", ad.CreatedAt == default ? DateTime.Now : ad.CreatedAt);

            cmd.ExecuteNonQuery();
        }

        public void Update(Advertisement ad)
        {
            const string sql = @"
                UPDATE Advertisements
                SET Title = @Title,
                    Description = @Description,
                    Price = @Price,
                    SellerId = @SellerId,
                    CategoryId = @CategoryId
                WHERE Id = @Id;";

            using var conn = _connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Title", ad.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", ad.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", ad.Price);
            cmd.Parameters.AddWithValue("@SellerId", ad.SellerId);
            cmd.Parameters.AddWithValue("@CategoryId", ad.CategoryId);
            cmd.Parameters.AddWithValue("@Id", ad.Id);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            const string sql = "DELETE FROM Advertisements WHERE Id = @Id;";
            using var conn = _connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
