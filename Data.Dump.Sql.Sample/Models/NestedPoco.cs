using System.Collections.Generic;

namespace Data.Dump.Sql.Sample.Models
{
    internal class NestedPoco : Poco
    {
        public IEnumerable<Poco> Pocos { get; set; }
    }
}
