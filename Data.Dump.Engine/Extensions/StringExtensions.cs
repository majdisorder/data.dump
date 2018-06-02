using System.Text.RegularExpressions;

namespace Data.Dump.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex Alpha = new Regex(@"[^a-z_]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex AlphaNumeric = new Regex(@"[^a-z0-9_]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string Sanitize(this string me, bool keepNumeric = true)
        {
            if (keepNumeric)
            {
                return AlphaNumeric.Replace(me, string.Empty);
            }

            return Alpha.Replace(me, string.Empty);
        }
    }
}
