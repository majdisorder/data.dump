using System.Collections.Generic;

namespace Data.Dump.Postgres.Sample.Models
{
    internal class NestedPoco : Poco
    {
        public IEnumerable<Poco> Pocos { get; set; }
    }
}
