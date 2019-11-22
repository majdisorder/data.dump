using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Dump.Postgres.Sample.Models;

namespace Data.Dump.Postgres.Sample.Data
{
    internal static class DataFactory
    {
        public static IEnumerable<Poco> Pocos(int amount, int offset = 1)
        {
            for (var i = 0; i < amount; ++i)
            {
                var position = i + offset;
                yield return new Poco
                {
                    Id = position,
                    Title = $"Poco #{position}",
                    Key = Guid.NewGuid(),
                    Created = DateTime.Now,
                    DateTime = DateTime.Now.AddHours(position),
                    Time = TimeSpan.FromHours(position),
                    Decimal = position + (decimal)Math.PI,
                    Fraction = position / (float)Math.PI,
                    Number = position,
                    LargeNumber = long.MaxValue - position,
                    Binary = Encoding.UTF8.GetBytes($"Poco #{position}")
                };
            }
        }

        public static IEnumerable<NestedPoco> NestedPocos(int amount, int offset = 1)
        {
            for (var i = 0; i < amount; ++i)
            {
                var position = i + offset;
                yield return new NestedPoco
                {
                    Id = position,
                    Title = $"NestedPoco #{position}",
                    Key = Guid.NewGuid(),
                    Created = DateTime.Now,
                    DateTime = DateTime.Now.AddHours(position),
                    Time = TimeSpan.FromHours(position),
                    Decimal = position + (decimal)Math.PI,
                    Fraction = position / (float)Math.PI,
                    Number = position,
                    LargeNumber = long.MaxValue - position,
                    Binary = Encoding.UTF8.GetBytes($"NestedPoco #{position}"),
                    Pocos = Pocos(amount, 1 + i * amount)
                };
            }
        }

        public static IEnumerable<DeeplyNestedPoco> DeeplyNestedPocos(int amount, int offset = 1)
        {
            for (var i = 0; i < amount; ++i)
            {
                var position = i + offset;
                yield return new DeeplyNestedPoco
                {
                    Id = position,
                    Title = $"DeeplyNestedPoco #{position}",
                    Key = Guid.NewGuid(),
                    Created = DateTime.Now,
                    DateTime = DateTime.Now.AddHours(position),
                    Time = TimeSpan.FromHours(position),
                    Decimal = position + (decimal)Math.PI,
                    Fraction = position / (float)Math.PI,
                    Number = position,
                    LargeNumber = long.MaxValue - position,
                    Binary = Encoding.UTF8.GetBytes($"DeeplyNestedPoco #{position}"),
                    NestedPocos = NestedPocos(amount, 1 + i * amount)
                };
            }
        }

        public static IEnumerable<ComplexPoco> ComplexPocos(int amount, int offset = 1)
        {
            for (var i = 0; i < amount; ++i)
            {
                var position = i + offset;
                yield return new ComplexPoco
                {
                    Id = position,
                    Title = $"ComplexPoco #{position}",
                    Key = Guid.NewGuid(),
                    Created = DateTime.Now,
                    DateTime = DateTime.Now.AddHours(position),
                    Time = TimeSpan.FromHours(position),
                    Decimal = position + (decimal)Math.PI,
                    Fraction = position / (float)Math.PI,
                    Number = position,
                    LargeNumber = long.MaxValue - position,
                    Binary = Encoding.UTF8.GetBytes($"ComplexPoco #{position}"),
                    SimpleCollection = Pocos(amount, 1 + i * amount).Select(x => x.Title),
                    ComplexCollection = Pocos(amount, 1 + i * amount),
                    SimpleDictionary = Pocos(amount, 1 + i * amount).ToDictionary(x => x.Title, x => x.Title),
                    ComplexDictionary = Pocos(amount, 1 + i * amount).ToDictionary(x => x.Title, x => x),
                    InnerPoco = Pocos(1, amount + 1 + i * amount).First()
                };
            }
        }
    }
}
