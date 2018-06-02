using Data.Dump.Extensions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Data.Dump.Schema.Sql
{
    public class SqlTableDefinitionGenerator : TableDefinitionGenerator, ISqlTableDefinitionGenerator
    {
        private static readonly Regex SqlNamePattern = new Regex(@"^\[?([^\]]+)\]?$", RegexOptions.Compiled);
        private static readonly SqlDbType[] MaxSizeTypes =
        {
            SqlDbType.NVarChar, SqlDbType.VarChar, SqlDbType.Binary, SqlDbType.VarBinary
        };

        private SqlDbType GetSqlDbType(Type type)
        {
            return new SqlParameter
            {
                DbType = GetDbType(type)
            }.SqlDbType;
        }

        public override string GetDbType(DataColumn column)
        {
            var sqlType = GetSqlDbType(column.DataType);
            if (MaxSizeTypes.Contains(sqlType))
            {
                return $"{sqlType}({(column.MaxLength >= 0 ? column.MaxLength.ToString() : "max")})";
            }

            if (sqlType == SqlDbType.Decimal)
            {
                return $"{sqlType}(18,9)";
            }

            return sqlType.ToString();
        }

        public override string GetValidName(string objectName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                throw new ArgumentNullException(nameof(objectName));
            }

            objectName = objectName.Sanitize();

            return SqlNamePattern.Replace(
                objectName.Length > 85 ? objectName.Substring(0, 85) : objectName, 
                "[$1]"
            );
        }

        public override string GetColumnDefinition(DataColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            return $"{GetValidName(column.ColumnName)} " +
                   $"{GetDbType(column)} " +
                   $"{(column.AutoIncrement ? $"IDENTITY({column.AutoIncrementSeed}, {column.AutoIncrementStep})" : string.Empty)}" +
                   $"{(column.AllowDBNull ? string.Empty : "not ")}null";
        }
    }
}
