using Data.Dump.Schema;
using Data.Dump.Schema.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Data.Dump.Persistence
{
    public abstract class RepositoryBase : IRepository
    {
        protected readonly IStore Store;
        protected readonly ITableDefinitionGenerator TableDefinitionGenerator;
        protected readonly IDataTableFactory DataTableFactory;
        protected readonly IDataSetFactory DataSetFactory;

        protected RepositoryBase(
            IStore store,
            ITableDefinitionGenerator tableDefinitionGenerator,
            IDataTableFactory dataTableFactory, IDataSetFactory dataSetFactory)
        {
            Store = store;
            TableDefinitionGenerator = tableDefinitionGenerator;
            this.DataTableFactory = dataTableFactory;
            DataSetFactory = dataSetFactory;
        }

        /// <summary>
        /// Write the data in this set to temptables.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tempTableMap">Optional live to temptable name map. 
        /// If no matching names are found, temptables will automatically be generated.</param>
        /// <returns>A tuple with the names of the live and temp tables.</returns>
        protected IList<TablePair> Write(DataSet data, IEnumerable<TablePair> tempTableMap = null)
        {
            return Write(Store.OpenSession(), data, tempTableMap);
        }

        /// <summary>
        /// Write the data in this table to a temptable.
        /// </summary>
        /// <param name="table">The data.</param>
        /// <param name="tempTableName">Optional temptable name. 
        /// If no name is passed in, a temptable will automatically be generated.</param>
        /// <returns>A tuple with the names of the live and temp tables.</returns>
        protected TablePair Write(DataTable table, string tempTableName = null)
        {
            return Write(Store.OpenSession(), table, tempTableName);
        }

        /// <summary>
        /// Write the data in this set to temptables using the passed in connection. 
        /// Note: Preferably used through one of the overloads. If you don't use the overloads, make sure to dispose the connection. 
        /// </summary>
        /// <param name="connection">The connection to use. Will not be disposed.</param>
        /// <param name="data">The data.</param>
        /// <param name="tempTableMap">Optional live to temptable name map. 
        /// If no matching names are found, temptables will automatically be generated.</param>
        /// <returns>A tuple with the names of the live and temp tables.</returns>
        protected abstract IList<TablePair> Write(IDbConnection connection, DataSet data, IEnumerable<TablePair> tempTableMap = null);

        /// <summary>
        /// Write the data in this table to a temptable using the passed in connection. 
        /// Note: Preferably used through one of the overloads. If you don't use the overloads, make sure to dispose the connection. 
        /// </summary>
        /// <param name="connection">The connection to use. Will not be disposed.</param>
        /// <param name="table">The data.</param>
        /// <param name="tempTableName">Optional temptable name. 
        /// If no name is passed in, a temptable will automatically be generated </param>
        /// <returns>A tuple with the names of the live and temp tables.</returns>
        protected abstract TablePair Write(IDbConnection connection, DataTable table, string tempTableName = null);

        /// <summary>
        /// Sets temptable as live and disposes of the current live data.
        /// </summary>
        /// <param name="tempTableName">Name of the temptable</param>
        /// <param name="liveTableName">Name of the live table</param>
        protected void GoLive(string tempTableName, string liveTableName)
        {
            using (var connection = Store.OpenSession())
            {
                GoLive(connection, tempTableName, liveTableName);
            }
        }

        /// <summary>
        /// Sets temptable as live and disposes of the current live data using the passed in connection.
        /// Note: Preferably used through one of the overloads. If you don't use the overloads, make sure to dispose the connection. 
        /// </summary>
        /// <param name="connection">The connection to use. Will not be disposed.</param>
        /// <param name="tempTableName">Name of the temptable</param>
        /// <param name="liveTableName">Name of the live table</param>
        protected abstract void GoLive(IDbConnection connection, string tempTableName, string liveTableName);

        protected abstract IDbCommand GetCreateTableCommand(IDbConnection connection, DataTable table);

        protected abstract IDbCommand CreateDbCommand(
            IDbConnection connection, string query, IDictionary<string, object> parameters = null,
            CommandType commandType = CommandType.Text);

        protected abstract void AddParameters(IDbCommand command, IDictionary<string, object> parameters);

        protected virtual string EnsureTableName(DataTable table)
        {
            if (string.IsNullOrWhiteSpace(table.TableName))
            {
                table.TableName = TableDefinitionGenerator
                    .GetValidName($"Table_{Guid.NewGuid():N}");
            }

            return table.TableName;
        }

        protected virtual string GetTempTableName(DataTable table)
        {
            return TableDefinitionGenerator.GetValidName(
                $@"tmp_{TableDefinitionGenerator.GetValidName(EnsureTableName(table)).Trim('[', ']')}_{Guid.NewGuid():N}"
            );
        }

        /// <summary>
        /// Makes sure the temptable exists in the database. 
        /// </summary>
        /// <param name="table">The original DataTable.</param>
        /// <returns>An empty table with the correct schema and name.</returns>
        protected DataTable CreateTempSchema(DataTable table)
        {
            return CreateTempSchema(Store.OpenSession(), table);
        }

        /// <summary>
        /// Makes sure the temptable exists in the database using the passed in connection. 
        /// Note: Preferably used through one of the overloads. If you don't use the overloads, make sure to dispose the connection. 
        /// </summary>
        /// <param name="connection">The connection to use. Will not be disposed.</param>
        /// <param name="table">The original DataTable.</param>
        /// <returns>An empty table with the correct schema and name.</returns>
        protected virtual DataTable CreateTempSchema(IDbConnection connection, DataTable table)
        {
            var tmpTable = table.Clone();
            tmpTable.TableName = GetTempTableName(table);

            var createTable = GetCreateTableCommand(connection, tmpTable);
            createTable.ExecuteNonQuery();

            return tmpTable;
        }

        /// <inheritdoc />
        public void Save<T>(T data, FieldSelectorCollection<T> fieldSelectors)
            where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Save<T>(new[] { data }, fieldSelectors);
        }

        /// <inheritdoc />
        public void Save<T>(IEnumerable<T> data, FieldSelectorCollection<T> fieldSelectors)
            where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var results = DataSetFactory
                .Create(data, fieldSelectors)
                .Where(set => set != null)
                .Aggregate<DataSet, IList<TablePair>>(
                    null, 
                    (current, set) => Write(set, current)
                );

            if (results == null) return;

            foreach (var result in results)
            {
                GoLive(result.TempTable, result.LiveTable);
            }
        }

        /// <inheritdoc />
        public virtual void Save<T>(T data, string tableName = null)
            where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Save<T>(new[] { data }, tableName);
        }

        /// <inheritdoc />
        public virtual void Save<T>(IEnumerable<T> data, string tableName = null)
            where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var result = DataTableFactory
                .Create(data, tableName)
                .Where(table => table != null)
                .Aggregate<DataTable, TablePair>(
                    null, 
                    (current, table) => Write(table, current?.TempTable)
                );

            if (result != null)
            {
                GoLive(result.TempTable, result.LiveTable);
            }
        }

        /// <inheritdoc />
        public virtual void Save(DataSet data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var results = Write(data);
            foreach (var result in results)
            {
                GoLive(result.TempTable, result.LiveTable);
            }
        }

        /// <inheritdoc />
        public virtual void Save(DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var result = Write(table);
            GoLive(result.TempTable, result.LiveTable);
        }
    }
}
