using System.Data;
using System.Data.SqlClient;

namespace ITSG.Excersise.Infrastructure.DB
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private IDbConnection? _dbConnection;
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            _dbConnection = new SqlConnection(_connectionString);
            _dbConnection.Open();

            return _dbConnection;
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
