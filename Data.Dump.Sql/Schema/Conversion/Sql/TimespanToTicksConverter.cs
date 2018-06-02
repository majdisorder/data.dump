using System;
using System.Reflection;

namespace Data.Dump.Schema.Conversion.Sql
{
    internal class TimespanToTicksConverter : ValueConverter
    {
        public TimespanToTicksConverter() 
            : base(typeof(TimeSpan?))
        {
        }

        public override object Convert(object value, PropertyInfo property)
        {
            if (value is TimeSpan time)
            {
                return time.Ticks;
            }

            return value;
        }
    }
}
