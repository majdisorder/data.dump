using NpgsqlTypes;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Data.Dump.Data
{
    internal class PostgresDbTypeResolver
    {
        private static readonly ConcurrentDictionary<NpgsqlDbType, string> TypeMap = new ConcurrentDictionary<NpgsqlDbType, string>();

        static PostgresDbTypeResolver()
        {
            var props = typeof(NpgsqlDbType).GetFields();
            PropertyInfo nameProp = null;

            foreach (var fieldInfo in props)
            {
                var attrs = fieldInfo.GetCustomAttributes(false);

                foreach (var attr in attrs.Where(x => x.GetType().FullName.Contains("BuiltInPostgresType")))
                {
                    if (nameProp == null)
                    {
                        nameProp = attr.GetType().GetProperty("Name",
                            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    }

                    TypeMap.TryAdd(
                        (NpgsqlDbType)fieldInfo.GetValue(fieldInfo),
                        nameProp.GetValue(attr).ToString()
                    );
                }
            }
        }

        public string Resolve(NpgsqlDbType type)
            => TypeMap.TryGetValue(type, out var postgresType) ?
                postgresType :
                "text";

    }
}
