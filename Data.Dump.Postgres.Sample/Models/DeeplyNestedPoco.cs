using System.Collections.Generic;

namespace Data.Dump.Postgres.Sample.Models
{
    internal class DeeplyNestedPoco : Poco
    {
        public IEnumerable<NestedPoco> NestedPocos { get; set; }
        public Poco Poco { get; set; }
    }
}
