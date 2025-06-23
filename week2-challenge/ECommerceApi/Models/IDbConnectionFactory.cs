using System.Data;

namespace ECommerceApi.Models
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
} 