namespace Data.Dump.Schema.Mapping
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