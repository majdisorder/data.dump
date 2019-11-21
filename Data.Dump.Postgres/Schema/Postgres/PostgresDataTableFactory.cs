using System;
using System.Data;
using Data.Dump.Schema.Conversion.Postgres;

namespace Data.Dump.Schema.Postgres
{
    ///<inheritdoc cref="DataTableFactory" />
    /// <summary>
    /// Default implmentation of DataTableFactory using ISqlTableDefinitionGenerator
    /// </summary>
    public class PostgresDataTableFactory : DataTableFactory, IPostgresDataTableFactory
    { 
        public PostgresDataTableFactory(IPostgresTableDefinitionGenerator tableDefinitionGenerator) 
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
