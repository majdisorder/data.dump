using Data.Dump.Schema;
using System.Collections.Generic;
using System.Data;

namespace Data.Dump.Persistence
{
    public interface IRepository
    {
        /// <summary>
        /// Save a single model as a table. Nested types contained in <paramref name="fieldSelectors"/> will be saved as seperate tables.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="data">The data to save.</param>
        /// <param name="fieldSelectors">The fields to be transformed to datatables. Make sure to include the base object.</param>
        void Save<T>(T data, FieldSelectorCollection<T> fieldSelectors)
            where T : class;

        /// <summary>
        /// Save a collection of models as a table. Nested types contained in <paramref name="fieldSelectors"/> will be saved as seperate tables. 
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="data">The data to save.</param>
        /// <param name="fieldSelectors">The fields to be transformed to datatables. Make sure to include the base object.</param>
        void Save<T>(IEnumerable<T> data, FieldSelectorCollection<T> fieldSelectors)
            where T : class;

        /// <summary>
        /// Save a single model as a table. Nested types will not be saved. 
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="data">The data to save.</param>
        /// <param name="tableName">Optional tablename. If left empty the name will be resolved based on type T.</param>
        void Save<T>(T data, string tableName = null)
            where T : class;

        /// <summary>
        /// Save a collection of models as a table. Nested types will not be saved. 
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="data">The data to save.</param>
        /// <param name="tableName">Optional tablename. If left empty the name will be resolved based on type T.</param>
        void Save<T>(IEnumerable<T> data, string tableName = null)
            where T : class;

        /// <summary>
        /// Save a set of tables as tables.
        /// </summary>
        /// <param name="data">The data to save.</param>
        void Save(DataSet data);

        /// <summary>
        /// Save a single table as a table.
        /// </summary>
        /// <param name="table">The data to save.</param>
        void Save(DataTable table);
    }
}