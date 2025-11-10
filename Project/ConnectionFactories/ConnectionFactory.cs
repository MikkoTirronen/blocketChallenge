namespace BlocketChallenge.Project.ConnectionFactories;

using Microsoft.Data.SqlClient;
public class DbConnectionFactory(string connectionString)
{
    private readonly string _connectionString = connectionString;


    public SqlConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}