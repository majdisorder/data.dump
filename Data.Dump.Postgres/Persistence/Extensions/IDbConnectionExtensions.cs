using Npgsql;
using System;
using System.Data;

namespace Data.Dump.Persistence.Extensions
{
    internal static class IDbConnectionExtensions
    {
        public static NpgsqlConnection AsNpgsqlConnection(this IDbConnection connection)
        {
            if (connection is NpgsqlConnection sqlConnection)
            {
                return sqlConnection;
            }

            throw new ArgumentException(
                $"IDbConnection instance can not be converted to {nameof(NpgsqlConnection)}.",
                nameof(connection)
            );
        }
    }
}
