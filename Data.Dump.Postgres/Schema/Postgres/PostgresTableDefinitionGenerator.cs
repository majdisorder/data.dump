using Data.Dump.Extensions;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Text.RegularExpressions;

namespace Data.Dump.Schema.Postgres
{
    public class PostgresTableDefinitionGenerator : TableDefinitionGenerator, IPostgresTableDefinitionGenerator
    {
        private static readonly Regex PostgresNamePattern = new Regex(@"^""?([^""]]+)""?$", RegexOptions.Compiled);
        //private static readonly SqlDbType[] MaxSizeTypes =
        //{
        //    SqlDbType.NVarChar, SqlDbType.VarChar, SqlDbType.Binary, SqlDbType.VarBinary
        //};

        private NpgsqlDbType GetNpgsqlDbType(Type type)
        {
            return new NpgsqlParameter
            {
                DbType = GetDbType(type)
            }.NpgsqlDbType;
        }

        private string GetBuiltInPostgresType(NpgsqlDbType type)
        {
            NpgsqlDbType.BuiltInPostgresType
        }

        public override string GetDbType(DataColumn column)
        {
            var sqlType = GetNpgsqlDbType(column.DataType);
            //if (MaxSizeTypes.Contains(sqlType))
            //{
            //    return $"{sqlType}({(column.MaxLength >= 0 ? column.MaxLength.ToString() : "max")})";
            //}

            //if (sqlType == SqlDbType.Decimal)
            //{
            //    return $"{sqlType}(18,9)";
            //}

            return sqlType.ToString();
        }

        public override string GetValidName(string objectName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                throw new ArgumentNullException(nameof(objectName));
            }

            objectName = objectName.Sanitize();

            return PostgresNamePattern.Replace(
                objectName.Length > 85 ? objectName.Substring(0, 85) : objectName, 
                "\"$1\""
            );
        }

        public override string GetColumnDefinition(DataColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            //TODO: support for seed and step in identity columns
            //https://chartio.com/resources/tutorials/how-to-define-an-auto-increment-primary-key-in-postgresql/

            return $"{GetValidName(column.ColumnName)} " +
                   $"{GetDbType(column)} " +
                   $"{(column.AutoIncrement ? "generated always as identity" : string.Empty)}" +
                   $"{(column.AllowDBNull ? string.Empty : "not ")}null";
        }
    }
}
