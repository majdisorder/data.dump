using System;
using System.Reflection;

namespace Data.Dump.Schema
{
    public interface IValueConverter
    {
        object Convert(object value, PropertyInfo property);
        Type ForType { get; }
    }
}