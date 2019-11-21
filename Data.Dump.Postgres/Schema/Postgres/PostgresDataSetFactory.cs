using Data.Dump.Schema.Conversion.Postgres;
using System;
using System.Data;

namespace Data.Dump.Schema.Postgres
{
    public class PostgresDataSetFactory : DataSetFactory, IPostgresDataSetFactory
    {    ///<inheritdoc cref="DataSetFactory" />
         /// <summary>
         /// Default implementation of DataSetFactory using IPostgresTableDefinitionGenerator
         /// </summary>
        public PostgresDataSetFactory(IPostgresTableDefinitionGenerator tableDefinitionGenerator) 
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
