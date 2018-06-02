using System.Collections.Generic;
using System.Data;

namespace Data.Dump.Schema
{
    public interface IDataSetFactory
    {
        /// <summary>
        /// Create a dataset based on type T and fill it. Only supported datatypes will be transformed to columns. Nested types contained in <paramref name="fieldSelectors"/> will be added as seperate datatables.
        /// </summary>
        /// <typeparam name="T">Type of the model to be transformed.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="fieldSelectors">The fields to be transformed to datatables. Make sure to include the base object.</param>
        /// <returns></returns>
        IEnumerable<DataSet> Create<T>(IEnumerable<T> data, FieldSelectorCollection<T> fieldSelectors, int dumpEvery = 100000)
            where T : class;
    }
}