using Data.Dump.Data;
using Data.Dump.Persistence.Extensions;
using Data.Dump.Schema.Postgres;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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

        private static void Write(PostgresBulkCopy bulkCopy, DataTable table, string destinationTableName)
        {
            bulkCopy.DestinationTableName = destinationTableName;
            bulkCopy.WriteToServer(table, DataRowState.Added);
        }

        protected override IList<TablePair> Write(IDbConnection connection, DataSet data, IEnumerable<TablePair> tempTableMap = null)
        {
            var result = new List<TablePair>();
            var bulkCopy = new PostgresBulkCopy(connection.AsNpgsqlConnection());
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
            var bulkCopy = new PostgresBulkCopy(connection.AsNpgsqlConnection());
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                tempTableName = CreateTempSchema(connection, table).TableName;
            }

            Write(bulkCopy, table, tempTableName);
            return new TablePair(table.TableName, tempTableName);
        }

        protected override void GoLive(IDbConnection connection, string tempTableName, string liveTableName)
        {
            var functionId = $"{Guid.NewGuid():N}";
            var command = CreateDbCommand(
                connection,
                "start transaction;"+
                $"create or replace function create_{functionId} ()\r\n" +
                "returns void as\r\n" +
                "$func$\r\n" +
                "begin\r\n" +
                "start transaction;\r\n" +
                $"if exists (select to_regclass('{tempTableName}') is not null) then\r\n" +
                $"if exists (select to_regclass('{liveTableName}') is not null) then\r\n" +
                $"drop table {liveTableName};\r\n" +
                "end if;\r\n" +
                $"alter table {tempTableName} rename to {liveTableName};\r\n" +
                "end if;\r\n" +
                "commit transaction;\r\n" +
                "end\r\n" +
                "$func$ language plpgsql;\r\n" +
                $"select  create_{functionId} ();\r\n"+
                $"drop function create_{functionId};\r\n" +
                "commit transaction;"
            );
            
            command.ExecuteNonQuery();
        }

        protected override IDbCommand GetCreateTableCommand(IDbConnection connection, DataTable table)
        {
            var functionId = $"{Guid.NewGuid():N}";
            return CreateDbCommand(
                connection,
                "start transaction;\r\n" +
                $"create or replace function create_{functionId} ()\r\n" +
                "returns void as\r\n" +
                "$func$\r\n" +
                "begin\r\n" +
                $"if (select to_regclass('{table.TableName}') is null) then\r\n" +
                $"create {TableDefinitionGenerator.GetTableDefinition(table)};\r\n" +
                "end if;\r\n" +
                "end\r\n" +
                "$func$ language plpgsql;\r\n" +
                $"select  create_{functionId} ();\r\n" +
                $"drop function create_{functionId};\r\n" +
                "commit transaction;\r\n"
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
