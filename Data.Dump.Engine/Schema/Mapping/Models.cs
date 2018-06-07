namespace Data.Dump.Schema.Mapping
{
    public static class Models
    {
        public static FieldSelectorCollection<TSource> Map<TSource>()
            where TSource : class
        {
            return new FieldSelectorCollection<TSource>();
        }
    }
}
