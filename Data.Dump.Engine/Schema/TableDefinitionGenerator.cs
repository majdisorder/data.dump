using Data.Dump.Extensions;
using System;
using System.Data;
using System.Linq;

namespace Data.Dump.Schema
{
    public abstract class TableDefinitionGenerator : ITableDefinitionGenerator
    {
        public virtual string GetTableDefinition(DataTable table)
        {
            return $"table {GetValidName(table.TableName)}{GetColumnDefinition(table)}";
        }

        public abstract string GetColumnDefinition(DataColumn column);

        public virtual string GetColumnDefinition(DataTable table)
        {
            var def = string.Join(
                ", ",
                table
                    .Columns
                    .OfType<DataColumn>()
                    .Select(GetColumnDefinition)
            );

            return $"({def})";
        }

        public abstract string GetValidName(string objectName);

        public abstract string GetDbType(DataColumn column);

        protected virtual DbType GetDbType(Type type)
        {
            var dbType = ConvertTypeCodeToDbType(Type.GetTypeCode(type));
            if (dbType != DbType.Object)
            {
                return dbType;
            }

            if (type.IsAssignableTo(typeof(Guid?)))
                return DbType.Guid;
            if (type.IsAssignableTo(typeof(TimeSpan?)))
                return DbType.Time;
            if (type.IsAssignableTo(typeof(byte[])))
                return DbType.Binary;

            return DbType.String;
        }

        protected static DbType ConvertTypeCodeToDbType(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Char:
                    return DbType.StringFixedLength;
                case TypeCode.SByte:
                    return DbType.SByte;
                case TypeCode.Byte:
                    return DbType.Byte;
                case TypeCode.Int16:
                    return DbType.Int16;
                case TypeCode.UInt16:
                    return DbType.UInt16;
                case TypeCode.Int32:
                    return DbType.Int32;
                case TypeCode.UInt32:
                    return DbType.UInt32;
                case TypeCode.Int64:
                    return DbType.Int64;
                case TypeCode.UInt64:
                    return DbType.UInt64;
                case TypeCode.Single:
                    return DbType.Single;
                case TypeCode.Double:
                    return DbType.Double;
                case TypeCode.Decimal:
                    return DbType.Decimal;
                case TypeCode.DateTime:
                    return DbType.DateTime;
                case TypeCode.String:
                    return DbType.String;

                default:
                    return DbType.Object;
            }
        }
    }
}
