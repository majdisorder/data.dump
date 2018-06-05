using System;
using System.Reflection;
using Data.Dump.Schema.Mapping;

namespace Data.Dump.Schema.Conversion
{
    public abstract class ValueConverter : IValueConverter
    {
        protected ValueConverter(Type forType)
        {
            ForType = forType;
        }

        public abstract object Convert(object value, PropertyInfo property);
        public Type ForType { get; }
    }
}
