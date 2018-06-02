using System;

namespace Data.Dump.Sql.Sample.Models
{
    internal class Poco
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DateTime { get; set; }
        public TimeSpan? Time { get; set; }
        public int? Number { get; set; }
        public long? LargeNumber { get; set; }
        public float? Fraction { get; set; }
        public decimal? Decimal { get; set; }
        public byte[] Binary { get; set; }
    }
}
