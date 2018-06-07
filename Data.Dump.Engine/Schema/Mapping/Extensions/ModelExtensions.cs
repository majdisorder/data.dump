namespace Data.Dump.Schema.Mapping.Extensions
{
    public static class ModelExtensions
    {
        public static  IForeignKeySelect<TParent, TModel> WithRoot<TModel, TParent>(this TModel me, TParent source)
            where TModel : class 
            where TParent : class
        {
            return new ForeignKeyModelPair<TParent, TModel>(source, me);
        }
    }
}
