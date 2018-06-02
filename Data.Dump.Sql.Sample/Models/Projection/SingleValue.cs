namespace Data.Dump.Sql.Sample.Models.Projection
{
    internal class SingleValue<T>
    {
        public SingleValue(T value)
        {
            Value = value;
        }
        public T Value { get; set; }
    }
}