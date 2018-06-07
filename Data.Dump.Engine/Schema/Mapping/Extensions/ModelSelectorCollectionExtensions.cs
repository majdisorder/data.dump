using Data.Dump.Extensions;
using System;
using System.Collections.Generic;

namespace Data.Dump.Schema.Mapping.Extensions
{
    public static class ModelSelectorCollectionExtensions
    {

        private static void ValidateModelType(Type type)
        {
            if (type.IsEnumerable() &&
                type.IsGenericType &&
                (type.GenericTypeArguments[0]).IsAssignableTo(typeof(IForeignKeySelect<,>)))
            {
                throw new ArgumentException($"Select {nameof(ModelExtensions.WithRoot)} is not available for {nameof(SelectModel)}. Use {nameof(SelectNestedModel)} instead.", nameof(type));
            }
        }

        #region simple selectors
        /// <summary>
        /// Select a model or collection of models to include in the dataset.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel>(field));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, string tableName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel>(field, tableName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<string> tableName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel>(field, tableName));

            return me;
        }

        #endregion simple selectors

        #region selectors with fk

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<TSource, TForeignKey> foreignKey)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, foreignKey));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<TSource, TForeignKey> foreignKey, string foreignKeyName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional function returning a name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<TSource, TForeignKey> foreignKey, Func<string> foreignKeyName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, string tableName, Func<TSource, TForeignKey> foreignKey)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, tableName, foreignKey));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, string tableName, Func<TSource, TForeignKey> foreignKey, string foreignKeyName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional function returning a name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, string tableName, Func<TSource, TForeignKey> foreignKey, Func<string> foreignKeyName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TForeignKey> foreignKey)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, tableName, foreignKey));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TForeignKey> foreignKey, string foreignKeyName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to the root collection.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional function returning a name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectModel<TSource, TModel, TForeignKey>(
            this FieldSelectorCollection<TSource> me, Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TForeignKey> foreignKey, Func<string> foreignKeyName)
            where TSource : class
            where TModel : class
        {
            ValidateModelType(typeof(TModel));
            me?.Add(new FieldSelector<TSource, TModel, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        #endregion selectors with fk

        #region nested selectors with fk

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me,
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field,
            Func<TForeignKeyRoot, TForeignKey> foreignKey)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, foreignKey));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field,
            Func<TForeignKeyRoot, TForeignKey> foreignKey, 
            string foreignKeyName)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional function returning a name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field,
            Func<TForeignKeyRoot, TForeignKey> foreignKey, 
            Func<string> foreignKeyName)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field, 
            string tableName,
            Func<TForeignKeyRoot, TForeignKey> foreignKey)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, tableName, foreignKey));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field, 
            string tableName,
            Func<TForeignKeyRoot, TForeignKey> foreignKey, 
            string foreignKeyName)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="tableName">Optional name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional function returning a name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field, 
            string tableName,
            Func<TForeignKeyRoot, TForeignKey> foreignKey, 
            Func<string> foreignKeyName)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field,
            Func<string> tableName, 
            Func<TForeignKeyRoot, TForeignKey> foreignKey)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, tableName, foreignKey));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field,
            Func<string> tableName, 
            Func<TForeignKeyRoot, TForeignKey> foreignKey, 
            string foreignKeyName)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        /// <summary>
        /// Select a model or collection of models to include in the dataset with a foreign key relating to a specific root.
        /// </summary>
        /// <typeparam name="TSource">Type of the root collection.</typeparam>
        /// <typeparam name="TModel">Type of the selected model.</typeparam>
        /// <typeparam name="TForeignKeyRoot">Type of the model containing the foreign key field.</typeparam>
        /// <typeparam name="TForeignKey">Type of the foreign key</typeparam>
        /// <param name="me">Collection of field selectors.</param>
        /// <param name="field">The field to include with its specific root.</param>
        /// <param name="tableName">Optional function returning a name for the resulting table.</param>
        /// <param name="foreignKey">Function returning the foreign key value.</param>
        /// <param name="foreignKeyName">Optional function returning a name for the foreign key column.</param>
        /// <returns>Collection of current the field selectors.</returns>
        public static FieldSelectorCollection<TSource> SelectNestedModel<TSource, TModel, TForeignKeyRoot, TForeignKey>(
            this FieldSelectorCollection<TSource> me, 
            Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyRoot, TModel>>> field,
            Func<string> tableName, 
            Func<TForeignKeyRoot, TForeignKey> foreignKey, 
            Func<string> foreignKeyName)
            where TSource : class
            where TModel : class
            where TForeignKeyRoot : class
        {
            me?.Add(new FieldSelector<TSource, TModel, TForeignKeyRoot, TForeignKey>(field, tableName, foreignKey, foreignKeyName));

            return me;
        }

        #endregion nested selectors with fk
    }
}
