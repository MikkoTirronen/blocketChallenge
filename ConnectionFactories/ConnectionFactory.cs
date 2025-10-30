namespace BlocketChallenge.ConnectionFactories;

using System.Data;
using Microsoft.Data.SqlClient;
public class DbConnectionFactory(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}