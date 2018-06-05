using Data.Dump.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Data.Dump.Schema.Mapping
{
    public class FieldSelectorCollection<TSource> : Collection<IFieldSelector<TSource>>
        where TSource : class
    {
    }

    public class FieldSelector<TSource, TModel>
        : IFieldSelector<TSource>
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

        public virtual object GetField(object root)
        {
            if(Field != null && root is TSource model)
            {
                return Field?.Invoke(model);
            }

            return null;
        }

        public Func<string> TableName { get; set; }

        protected static Type GetFieldType(Type type)
        {
            return
                !type.IsEnumerable() ? type : type.GetGenericEnumerableTypeArgument();
        }

        public Type FieldType { get; protected set; } = GetFieldType(typeof(TModel));
    }

    public class FieldSelector<TSource, TModel, TForeignKey>
        : FieldSelector<TSource, TModel>, IFieldSelectorWithForeignKey<TSource>
        where TSource : class
        where TModel : class
    {
        #region ctor

        public FieldSelector(Func<TSource, TModel> field, Func<TSource, TForeignKey> foreignKey)
            : base(field)
        {
            ForeignKey = foreignKey;
            ForeignKeyName = () => ForeignKeyNameValue;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<TSource, TForeignKey> foreignKey, string foreignKeyName)
            : this(field, foreignKey)
        {
            ForeignKeyNameValue = foreignKeyName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<TSource, TForeignKey> foreignKey,
            Func<string> foreignKeyName)
            : this(field, foreignKey)
        {
            ForeignKeyName = foreignKeyName;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName, Func<TSource, TForeignKey> foreignKey)
            : base(field, tableName)
        {
            ForeignKey = foreignKey;
            ForeignKeyName = () => ForeignKeyNameValue;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName, Func<TSource, TForeignKey> foreignKey,
            string foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyNameValue = foreignKeyName;
        }

        public FieldSelector(Func<TSource, TModel> field, string tableName, Func<TSource, TForeignKey> foreignKey,
            Func<string> foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyName = foreignKeyName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TForeignKey> foreignKey)
            : base(field, tableName)
        {
            ForeignKey = foreignKey;
            ForeignKeyName = () => ForeignKeyNameValue;

        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TForeignKey> foreignKey,
            string foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyNameValue = foreignKeyName;
        }

        public FieldSelector(Func<TSource, TModel> field, Func<string> tableName, Func<TSource, TForeignKey> foreignKey,
            Func<string> foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyName = foreignKeyName;
        }

        #endregion ctor

        private string ForeignKeyNameValue { get; }

        public object GetForeignKey(object root)
        {
            if (ForeignKey != null && root is TSource model)
            {
                return ForeignKey(model);
            }

            return null;
        }

        public Func<TSource, TForeignKey> ForeignKey { get; set; }

        public Func<string> ForeignKeyName { get; set; }

        public Type ForeignKeyType { get; } = typeof(TForeignKey);
    }
    
    public class FieldSelector<TSource, TModel, TForeignKeyModel, TForeignKey>
    : FieldSelector<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>>, IFieldSelectorWithForeignKey<TSource>
        where TSource : class
        where TModel : class
        where TForeignKeyModel : class
    {
        #region ctor

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, 
            Func<TForeignKeyModel, TForeignKey> foreignKey)
            : base(field)
        {
            ForeignKey = foreignKey;
            ForeignKeyName = () => ForeignKeyNameValue;
            FieldType = GetFieldType(typeof(TModel));
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, 
            Func<TForeignKeyModel, TForeignKey> foreignKey, string foreignKeyName)
            : this(field, foreignKey)
        {
            ForeignKeyNameValue = foreignKeyName;
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field,
            Func<TForeignKeyModel, TForeignKey> foreignKey,
            Func<string> foreignKeyName)
            : this(field, foreignKey)
        {
            ForeignKeyName = foreignKeyName;
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, string tableName,
            Func<TForeignKeyModel, TForeignKey> foreignKey)
            : base(field, tableName)
        {
            ForeignKey = foreignKey;
            ForeignKeyName = () => ForeignKeyNameValue;
            FieldType = GetFieldType(typeof(TModel));
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, string tableName, 
            Func<TForeignKeyModel, TForeignKey> foreignKey,
            string foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyNameValue = foreignKeyName;
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, string tableName, 
            Func<TForeignKeyModel, TForeignKey> foreignKey,
            Func<string> foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyName = foreignKeyName;
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, 
            Func<string> tableName, 
            Func<TForeignKeyModel, TForeignKey> foreignKey)
            : base(field, tableName)
        {
            ForeignKey = foreignKey;
            ForeignKeyName = () => ForeignKeyNameValue;
            FieldType = GetFieldType(typeof(TModel));
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, 
            Func<string> tableName, 
            Func<TForeignKeyModel, TForeignKey> foreignKey,
            string foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyNameValue = foreignKeyName;
        }

        public FieldSelector(Func<TSource, IEnumerable<IForeignKeySelect<TForeignKeyModel, TModel>>> field, 
            Func<string> tableName, 
            Func<TForeignKeyModel, TForeignKey> foreignKey,
            Func<string> foreignKeyName)
            : this(field, tableName, foreignKey)
        {
            ForeignKeyName = foreignKeyName;
        }

        #endregion ctor

        private string ForeignKeyNameValue { get; }

        public object GetForeignKey(object root)
        {
            if (ForeignKey != null && root is TForeignKeyModel model)
            {
                return ForeignKey(model);
            }

            return null;
        }

        public Func<TForeignKeyModel, TForeignKey> ForeignKey { get; set; }

        public Func<string> ForeignKeyName { get; set; }

        public Type ForeignKeyType { get; } = typeof(TForeignKey);
    }
}


