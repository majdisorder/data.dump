using System.Collections.Generic;

namespace Data.Dump.Sql.Sample.Models
{
    internal class DeeplyNestedPoco : Poco
    {
        public IEnumerable<NestedPoco> NestedPocos { get; set; }
        public Poco Poco { get; set; }
    }
}
