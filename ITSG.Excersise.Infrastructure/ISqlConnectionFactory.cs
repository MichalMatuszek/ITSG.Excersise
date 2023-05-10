using System.Data;

namespace ITSG.Excersise.Infrastructure
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
