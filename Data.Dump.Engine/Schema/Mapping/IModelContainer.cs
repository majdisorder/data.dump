using System.Collections.Generic;

namespace Data.Dump.Schema.Mapping
{
    internal interface IModelContainer
    {
        IEnumerable<object> GetModels();
    }
}