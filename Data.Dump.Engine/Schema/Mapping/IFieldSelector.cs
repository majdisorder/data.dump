using System;

namespace Data.Dump.Schema.Mapping
{
    public interface IFieldSelector<in TSource>
    where TSource : class
    {
        object GetField(object root);
        Func<string> TableName { get; }
        Type FieldType { get; }
    }

    public interface IFieldSelectorWithForeignKey<in TSource> : IFieldSelector<TSource>
        where TSource : class
    {
        object GetForeignKey(object root);
        Func<string> ForeignKeyName { get; }
        Type ForeignKeyType { get; }
    }
}