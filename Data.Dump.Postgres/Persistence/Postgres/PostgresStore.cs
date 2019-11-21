using Npgsql;
using System.Data;

namespace Data.Dump.Persistence.Postgres
{
    public class PostgresStore : IPostgresStore
    {
        protected readonly string ConnectionString;
       
        public PostgresStore(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public IDbConnection OpenSession()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            return connection;
        }
    }
}
