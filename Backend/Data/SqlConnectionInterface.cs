using System.Data;
using Microsoft.Data.SqlClient;
namespace AutoGovernance9Web.Backend.Data
{
    public class SqlConnectionInterface : IDbConnectionInterface
    {
        private readonly string _connectionString;

        public SqlConnectionInterface(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' was not found in app settings");
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
