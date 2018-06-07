namespace Data.Dump.Schema.Mapping
{
    public class SingleValue<T>
    {
        public SingleValue(T value)
        {
            Value = value;
        }
        public T Value { get; set; }
    }
}