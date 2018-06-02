using System.Collections.Generic;

namespace Data.Dump.Sql.Sample.Models
{
    internal class ComplexPoco : Poco
    {
        public Poco InnerPoco { get; set; }
        public IEnumerable<string> SimpleCollection { get; set; }
        public IEnumerable<Poco> ComplexCollection { get; set; }
        public IDictionary<string, string> SimpleDictionary { get; set; }
        public IDictionary<string, Poco> ComplexDictionary { get; set; }
    }
}
