using System;
using System.Collections.Generic;
using System.Data;

namespace Data.Dump.Schema
{
    /// <inheritdoc cref="IDataTableFactory" />
    public class DataTableFactory : DataContainerFactoryBase, IDataTableFactory
    {
        public DataTableFactory(ITableDefinitionGenerator tableDefinitionGenerator)
            : base(tableDefinitionGenerator)
        {
        }

        public virtual IEnumerable<DataTable> Create<T>(IEnumerable<T> data, string tableName = null, int dumpEvery = 100000)
            where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            
            return FillDataTable<T>(
                GetDataTableSchema(typeof(T), tableName), 
                data, 
                dumpEvery
            );
        }
    }
}
