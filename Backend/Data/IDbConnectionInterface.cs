using System.Data;

namespace AutoGovernance9Web.Backend.Data
{
    public interface IDbConnectionInterface
    {
        IDbConnection CreateConnection();
    }
}
