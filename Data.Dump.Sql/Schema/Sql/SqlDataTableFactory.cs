using Data.Dump.Schema.Conversion.Sql;
using System;
using System.Data;

namespace Data.Dump.Schema.Sql
{
    ///<inheritdoc cref="DataTableFactory" />
    /// <summary>
    /// Default implmentation of DataTableFactory using ISqlTableDefinitionGenerator
    /// </summary>
    public class SqlDataTableFactory : DataTableFactory, ISqlDataTableFactory
    { 
        public SqlDataTableFactory(ISqlTableDefinitionGenerator tableDefinitionGenerator) 
            : base(tableDefinitionGenerator)
        {
            ValueConverters.Add(new TimespanToTicksConverter());
        }

        protected override bool TryAddColumn(DataTable table, string name, Type type, out DataColumn column)
        {
            if (type.IsAssignableFrom(typeof(TimeSpan?)))
            {
                type = type.IsGenericType ? typeof(long?) : typeof(long);
            }

            return base.TryAddColumn(table, name, type, out column);
        }
    }
}
