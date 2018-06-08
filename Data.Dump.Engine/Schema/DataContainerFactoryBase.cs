using Data.Dump.Extensions;
using Data.Dump.Schema.Conversion;
using Data.Dump.Schema.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Data.Dump.Schema
{
    public abstract class DataContainerFactoryBase
    {
        protected class RowCreatedEventArgs : EventArgs
        {
            public RowCreatedEventArgs(DataRow row, object model)
            {
                Row = row;
                Model = model;
            }

            public DataRow Row { get; }
            public object Model { get; }
        }

        protected event EventHandler<RowCreatedEventArgs> RowCreated;
        protected static readonly IList<IValueConverter> ValueConverters = new List<IValueConverter>();
        protected static readonly Dictionary<Type, DataTable> GenericSchemaCache = new Dictionary<Type, DataTable>();
        protected static readonly IDictionary<Type, IDictionary<string, PropertyInfo>> KnownTypePropertyMap = new Dictionary<Type, IDictionary<string, PropertyInfo>>();
        protected static Type[] DbSupportedTypes =
        {
            typeof(Boolean), typeof(Byte), typeof(Byte[]), typeof(Char), typeof(DateTime), typeof(Decimal),
            typeof(Double), typeof(Guid), typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
            typeof(Single), typeof(String), typeof(TimeSpan), typeof(UInt16), typeof(UInt32), typeof(UInt64)
        };
        protected readonly ITableDefinitionGenerator TableDefinitionGenerator;

        protected DataContainerFactoryBase(ITableDefinitionGenerator tableDefinitionGenerator)
        {
            TableDefinitionGenerator = tableDefinitionGenerator;
        }

        private static object ApplyConversions(Type type, object value, PropertyInfo property = null)
        {
            value = ValueConverters
                .Where(x => type.IsAssignableTo(x.ForType))
                .Aggregate(
                    value,
                    (current, converter) => converter.Convert(current, property)
                );

            return value;
        }

        private void OnRowCreated(RowCreatedEventArgs e)
        {
            RowCreated?.Invoke(this, e);
        }

        private IEnumerable GetEnumerableModels(object item, bool isPrimitive)
        {
            var data = (item as IModelContainer)?.GetModels() ?? item;

            if (data is IEnumerable models && !isPrimitive)
            {
                return models;
            }

            return new[] { data };
        }

        protected virtual IEnumerable<DataTable> FillDataTable<T>(DataTable table, IEnumerable data, int dumpEvery)
            where T : class
        {
            return FillDataTable(typeof(T), table, data, dumpEvery);
        }

        protected virtual IEnumerable<DataTable> FillDataTable(Type type, DataTable table, IEnumerable data, int dumpEvery)
        {
            if (table != null && KnownTypePropertyMap.TryGetValue(type, out var propertyMap))
            {
                foreach (var item in data)
                {
                    var modelType = item.GetType();
                    var isPrimitive = IsDbSupportedType(modelType);

                    foreach (var model in GetEnumerableModels(item, isPrimitive))
                    {
                        var row = table.NewRow();

                        foreach (DataColumn column in table.Columns)
                        {
                            if (propertyMap.TryGetValue(column.ColumnName, out var property))
                            {
                                row[column] = (isPrimitive ? 
                                                    ApplyConversions(modelType, model) :
                                                    ApplyConversions(
                                                        property.PropertyType,
                                                        property.GetValue(model, null),
                                                        property
                                                    )
                                                ) ?? DBNull.Value;
                            }
                        }

                        OnRowCreated(new RowCreatedEventArgs(row, item));

                        table.Rows.Add(row);

                        if (table.Rows.Count == dumpEvery)
                        {
                            yield return table;
                            table.Rows.Clear();
                        }
                    }
                }
            }

            yield return table;
        }

        protected virtual DataTable GetDataTableSchema(Type type, string tableName = null)
        {
            lock (GenericSchemaCache)
            {
                if (!GenericSchemaCache.TryGetValue(type, out var table))
                {
                    table = new DataTable(GetTableName(type, tableName));
                    AddColumnsFromType(table, type);
                    if (table.Columns.Count == 0)
                    {
                        return null;
                    }

                    GenericSchemaCache.Add(type, table);
                }

                table = table.Clone();
                table.TableName = GetTableName(type, tableName);

                return table;
            }
        }

        protected virtual string GetTableName(Type type, string tableName)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                return TableDefinitionGenerator.GetValidName(tableName);
            }

            return TableDefinitionGenerator.GetValidName(type.GetReadableName());
        }

        protected virtual void AddColumnsFromType(DataTable table, Type type)
        {
            var propertyMap = new Dictionary<string, PropertyInfo>();

            var actualType = GetActualTypeIfPrimitive(type);

            foreach (var property in actualType.GetProperties())
            {
                if (TryAddColumn(table, property.Name, property.PropertyType, out var column))
                {
                    propertyMap.Add(column.ColumnName, property);
                }
            }

            //if we get here type should not be in KnownTypePropertyMap
            if (propertyMap.Keys.Any())
            {
                KnownTypePropertyMap.Add(type, propertyMap);
            }
        }

        protected virtual bool TryAddColumn(DataTable table, string name, Type type, out DataColumn column)
        {
            var actualType = GetActualTypeIfNullable(type);

            if (IsDbSupportedType(actualType.Type))
            {
                var colName = TableDefinitionGenerator.GetValidName(name);
                table.Columns.Add(
                    (column = new DataColumn(colName, actualType.Type)
                    {
                        AllowDBNull = actualType.IsNullable
                    })
                );

                return true;
            }

            column = null;
            return false;
        }

        protected virtual Type GetActualTypeIfPrimitive(Type type)
        {
            if (IsDbSupportedType(type))
            {
                return typeof(SingleValue<>)
                    .MakeGenericType(type);
            }

            return type;
        }

        protected virtual (Type Type, bool IsNullable) GetActualTypeIfNullable(Type type)
        {
            if (type.IsAssignableTo(typeof(Nullable<>)))
            {
                return (type.GenericTypeArguments[0], true);
            }

            return (type, type.IsClass);
        }

        protected virtual bool IsDbSupportedType(Type type)
        {
            return DbSupportedTypes.Contains(type);
        }
    }
}