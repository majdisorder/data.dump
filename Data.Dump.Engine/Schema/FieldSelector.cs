using Data.Dump.Extensions;
using System;
using System.Collections.ObjectModel;

namespace Data.Dump.Schema
{
    public class FieldSelectorCollection<TSource> : Collection<IFieldSelector<TSource>>
        where TSource : class
    { }

    public class FieldSelector<TSource, TModel> : IFieldSelector<TSource>
        where TSource : class
        where TModel : class
    {
        #region ctor
        public FieldSelector(Func<TSource, TModel> field) 
        {
            Field = field;
            TableName = () => TableNameValue;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName) 
            : this(field)
        {
            TableNameValue = tableName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName)
            : this(field)
        {
            TableName = tableName;
        }
        #endregion ctor

        private string TableNameValue { get; }
        public Func<TSource, TModel> Field { get; set; }

        public object GetField(TSource source)
        {
            return Field?.Invoke(source);
        }

        public Func<string> TableName { get; set; }

        protected static Type GetFieldType()
        {
            var type = typeof(TModel);

            return 
                !type.IsEnumerable() ? 
                    type : 
                    type.GetGenericEnumerableTypeArgument();
        }

        public Type FieldType { get; } = GetFieldType();
    }

    public class FieldSelector<TSource, TModel, TId> : FieldSelector<TSource, TModel>, IFieldSelectorWithParentId<TSource>
        where TSource : class
        where TModel : class
    {
        #region ctor
        public FieldSelector(Func<TSource, TModel> field, Func<TSource, TId> parentIdField) 
            : base(field)
        {
            ParentIdField = parentIdField;
            ParentIdFieldName = () => ParentIdFieldNameValue;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<TSource, TId> parentIdField, string parentIdFieldName)
            : this(field, parentIdField)
        {
            ParentIdFieldNameValue = parentIdFieldName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<TSource, TId> parentIdField, Func<string> parentIdFieldName)
            : this(field, parentIdField)
        {
            ParentIdFieldName = parentIdFieldName;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName, Func<TSource, TId> parentIdField) 
            : base(field, tableName)
        {
            ParentIdField = parentIdField;
            ParentIdFieldName = () => ParentIdFieldNameValue;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName, Func<TSource, TId> parentIdField, string parentIdFieldName)
            : this(field, tableName, parentIdField)
        {
            ParentIdFieldNameValue = parentIdFieldName;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName, Func<TSource, TId> parentIdField, Func<string> parentIdFieldName)
            : this(field, tableName, parentIdField)
        {
            ParentIdFieldName = parentIdFieldName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TId> parentIdField) 
            : base(field, tableName)
        {
            ParentIdField = parentIdField;
            ParentIdFieldName = () => ParentIdFieldNameValue;

        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TId> parentIdField, string parentIdFieldName)
            : this(field, tableName, parentIdField)
        {
            ParentIdFieldNameValue = parentIdFieldName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TId> parentIdField, Func<string> parentIdFieldName)
            : this(field, tableName, parentIdField)
        {
            ParentIdFieldName = parentIdFieldName;
        }
        #endregion ctor

        private string ParentIdFieldNameValue { get; }

        public object GetParentIdField(TSource root)
        {
           if (ParentIdField != null)
            {
                return ParentIdField(root);
            }
             
            return null;
        }

        public Func<TSource, TId> ParentIdField { get; set; }
       
        public Func<string> ParentIdFieldName { get; set; }

        public Type IdFieldType { get; } = typeof(TId);
    }
}
