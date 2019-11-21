using Data.Dump.Persistence.Extensions;
using Data.Dump.Schema.Postgres;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Dump.Persistence.Postgres
{
    /// <inheritdoc cref="RepositoryBase" />
    public class PostgresRepository : RepositoryBase, IPostgresRepository
    {
        public PostgresRepository(
            IPostgresStore store,
            IPostgresTableDefinitionGenerator tableDefinitionGenerator,
            IPostgresDataTableFactory dataTableFactory,
            IPostgresDataSetFactory dataSetFactory)
            : base(
                store, 
                tableDefinitionGenerator, 
                dataTableFactory,
                dataSetFactory)
        {
        }

        public PostgresRepository(
            IPostgresStore store)
            : this(
                store,
                new PostgresTableDefinitionGenerator(),
                new PostgresDataTableFactory(new PostgresTableDefinitionGenerator()),
                new PostgresDataSetFactory(new PostgresTableDefinitionGenerator()))
        {
        }

        private static void Write(SqlBulkCopy bulkCopy, DataTable table, string destinationTableName)
        {
            bulkCopy.DestinationTableName = destinationTableName;
            bulkCopy.WriteToServer(table, DataRowState.Added);
        }

        protected override IList<TablePair> Write(IDbConnection connection, DataSet data, IEnumerable<TablePair> tempTableMap = null)
        {
            var result = new List<TablePair>();
            var bulkCopy = new SqlBulkCopy(connection.AsNpgsqlConnection());
            var tempTables = tempTableMap?.ToDictionary(x => x.LiveTable, x => x.TempTable);
            string tempTableName = null;

            foreach (DataTable table in data.Tables)
            {
                if (!(tempTables?.TryGetValue(table.TableName, out tempTableName) ?? false) || 
                    string.IsNullOrWhiteSpace(tempTableName))
                {
                    tempTableName = CreateTempSchema(connection, table).TableName;
                }

                Write(bulkCopy, table, tempTableName);
                result.Add(new TablePair(table.TableName, tempTableName));
            }

            return result;
        }

        protected override TablePair Write(IDbConnection connection, DataTable table, string tempTableName = null)
        {
            var bulkCopy = new SqlBulkCopy(connection.AsNpgsqlConnection());
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                tempTableName = CreateTempSchema(connection, table).TableName;
            }

            Write(bulkCopy, table, tempTableName);
            return new TablePair(table.TableName, tempTableName);
        }

        protected override void GoLive(IDbConnection connection, string tempTableName, string liveTableName)
        {
            var command = CreateDbCommand(
                connection,
               "begin transaction; \r\n" +
                $"if OBJECT_ID('{tempTableName}') is not null \r\n" +
                "begin \r\n" +
                $"if OBJECT_ID('{liveTableName}') is not null \r\n" +
                "begin \r\n" +
                $"drop table {liveTableName}; \r\n" +
                "end \r\n" +
                $"exec sp_rename {tempTableName}, {liveTableName}; \r\n" +
                "end \r\n" +
                "commit transaction;"
            );
            
            command.ExecuteNonQuery();
        }

        protected override IDbCommand GetCreateTableCommand(IDbConnection connection, DataTable table)
        {
            return CreateDbCommand(
                connection,
                $"if OBJECT_ID('{table.TableName}') is null \r\n" +
                "begin \r\n" +
                $"create {TableDefinitionGenerator.GetTableDefinition(table)}; \r\n" +
                "end"
            );
        }

        protected override IDbCommand CreateDbCommand(
            IDbConnection connection, string query, IDictionary<string, object> parameters = null,
            CommandType commandType = CommandType.Text)
        {
            var sqlCommand = new NpgsqlCommand(query, connection.AsNpgsqlConnection())
            {
                CommandType = commandType,
                CommandTimeout = 0
            };

            AddParameters(sqlCommand, parameters);

            return sqlCommand;
        }

        protected override void AddParameters(IDbCommand command, IDictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
                }
            }
        }
    }
}
