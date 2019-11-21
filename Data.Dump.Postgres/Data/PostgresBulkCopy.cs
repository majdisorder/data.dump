using Npgsql;
using System.Data;

namespace Data.Dump.Data
{
    public class PostgresBulkCopy
    {
        public PostgresBulkCopy(NpgsqlConnection connection)
        {
            
        }

        public string DestinationTableName { get; set; }

        public void WriteToServer(DataTable table, DataRowState added)
        {
            throw new System.NotImplementedException();
        }
    }
}
