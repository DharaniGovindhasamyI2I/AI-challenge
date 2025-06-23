using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Models
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class PostgresConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
} 