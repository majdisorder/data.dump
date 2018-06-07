using System.Collections;
using System.Collections.Generic;

namespace Data.Dump.Schema.Mapping
{
    public class ForeignKeyModelPair<TFk, TModel> : IForeignKeySelect<TFk, TModel>, IModelContainer, IForeignKeyContainer
        where TModel : class
        where TFk : class
    {
        public ForeignKeyModelPair(TFk foreignKeyModel, TModel model)
        {
            ForeignKeyModel = foreignKeyModel;
            Model = model;
        }

        public ForeignKeyModelPair(KeyValuePair<TFk, TModel> keyValuePair)
        {
            ForeignKeyModel = keyValuePair.Key;
            Model = keyValuePair.Value;
        }

        public TFk ForeignKeyModel { get; set; }

        public TModel Model { get; set; }

        public object GetForeignKeyModel()
        {
            return ForeignKeyModel;
        }

        public IEnumerable<object> GetModels()
        {
            if (!(Model is IEnumerable models))
            {
                models = new[] {Model};
            }

            foreach (var model in models)
            {
                yield return model;
            }
        }
    }

    public interface IForeignKeySelect<TFk, TModel> 
        where TModel : class
        where TFk : class
    {
    }
}
