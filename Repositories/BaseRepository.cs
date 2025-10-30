using System.Data;
using BlocketChallenge.ConnectionFactories;
using Microsoft.Data.SqlClient;
namespace BlocketChallenge.Repositories;


public abstract class BaseRepository(DbConnectionFactory connectionFactory)
{
    protected readonly DbConnectionFactory? _connectionFactory = connectionFactory;

    protected int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
    {
        using var conn = _connectionFactory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }
        }
        return cmd.ExecuteNonQuery();
    }

    protected IDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
    {
        var conn = _connectionFactory.CreateConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        if (parameters != null)
        {
            foreach(var param in parameters)
            {
                cmd.Parameters.Add(param);
            }
        }
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }



}