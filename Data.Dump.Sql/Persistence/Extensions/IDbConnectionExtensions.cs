using System;
using System.Data;
using System.Data.SqlClient;

namespace Data.Dump.Persistence.Extensions
{
    internal static class IDbConnectionExtensions
    {
        public static SqlConnection AsSqlConnection(this IDbConnection connection)
        {
            if (connection is SqlConnection sqlConnection)
            {
                return sqlConnection;
            }

            throw new ArgumentException(
                $"IDbConnection instance can not be converted to {nameof(SqlConnection)}.",
                nameof(connection)
            );
        }
    }
}
