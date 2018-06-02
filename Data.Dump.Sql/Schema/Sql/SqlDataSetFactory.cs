using Data.Dump.Schema.Conversion.Sql;
using System;
using System.Data;

namespace Data.Dump.Schema.Sql
{
    public class SqlDataSetFactory : DataSetFactory, ISqlDataSetFactory
    {    ///<inheritdoc cref="DataSetFactory" />
         /// <summary>
         /// Default implmentation of DataSetFactory using ISqlTableDefinitionGenerator
         /// </summary>
        public SqlDataSetFactory(ISqlTableDefinitionGenerator tableDefinitionGenerator) 
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
