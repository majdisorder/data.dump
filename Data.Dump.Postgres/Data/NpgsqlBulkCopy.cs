using Data.Dump.Schema.Postgres;
using Npgsql;
using System.Data;
using System.Linq;

namespace Data.Dump.Data
{
    internal class NpgsqlBulkCopy
    {
        protected readonly IPostgresTableDefinitionGenerator TableDefinitionGenerator;
        protected readonly NpgsqlConnection Connection;

        public NpgsqlBulkCopy(NpgsqlConnection connection, IPostgresTableDefinitionGenerator tableDefinitionGenerator)
        {
            Connection = connection;
            TableDefinitionGenerator = tableDefinitionGenerator;
        }

        public string DestinationTableName { get; set; }

        public void WriteToServer(DataTable table)
        {
            using (var writer = Connection.BeginBinaryImport(GetCopyDefinition(table)))
            {
                foreach (DataRow row in table.Rows)
                {
                    writer.WriteRow(row.ItemArray);
                }
                writer.Complete();
            }
        }

        private string GetCopyDefinition(DataTable table)
        {
            var def = string.Join(
                ", ",
                table
                    .Columns
                    .OfType<DataColumn>()
                    .Select(x => TableDefinitionGenerator.GetValidName(x.ColumnName))
            );

            return $"COPY {DestinationTableName} ({def}) FROM STDIN (FORMAT BINARY)";
        }

       
    }

    
}
