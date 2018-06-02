using System;

namespace Data.Dump.Schema
{
    public interface IFieldSelector<in TSource>
    where TSource : class
    {
        object GetField(TSource source);
        Func<string> TableName { get; }
        Type FieldType { get; }
    }

    public interface IFieldSelectorWithParentId<in TSource> : IFieldSelector<TSource>
        where TSource : class
    {
        object GetParentIdField(TSource root);
        Func<string> ParentIdFieldName { get; }
        Type IdFieldType { get; }
    }
}