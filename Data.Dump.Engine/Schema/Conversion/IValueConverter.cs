using System;
using System.Reflection;

namespace Data.Dump.Schema.Conversion
{
    public interface IValueConverter
    {
        object Convert(object value, PropertyInfo property);
        Type ForType { get; }
    }
}