using System.Collections.Generic;
using System.Data;

namespace Data.Dump.Schema
{
    public interface IDataTableFactory
    {
        /// <summary>
        /// Create a datatable based on type T and fill it. Only supported datatypes will be transformed to columns. Nested types will be ignored.
        /// </summary>
        /// <typeparam name="T">Type of the model to be transformed.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="tableName">Optional tablename. If left empty the name will be resolved based on type T.</param>
        /// <returns></returns>
        IEnumerable<DataTable> Create<T>(IEnumerable<T> data, string tableName = null, int dumpEvery = 100000)
            where T : class;
    }
}