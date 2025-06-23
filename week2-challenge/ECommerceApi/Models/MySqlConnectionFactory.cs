using System.Data;
// using MySql.Data.MySqlClient; // Uncomment if you add MySQL NuGet package

namespace ECommerceApi.Models
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory(string connectionString) => _connectionString = connectionString;
        // public IDbConnection CreateConnection() => new MySqlConnection(_connectionString); // Uncomment if using MySQL
        public IDbConnection CreateConnection() => null; // Placeholder
    }
} 