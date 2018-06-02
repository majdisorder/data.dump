using System.Data;
using System.Data.SqlClient;

namespace Data.Dump.Persistence.Sql
{
    public class SqlStore : ISqlStore
    {
        protected readonly string ConnectionString;
       
        public SqlStore(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public IDbConnection OpenSession()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            return connection;
        }
    }
}
