using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Data.Dump.Extensions
{
    //from Raven.Client.Documents.Conventions
    internal class Inflector
    {
        private static readonly List<Inflector.Rule> Plurals = new List<Inflector.Rule>();
        private static readonly List<Inflector.Rule> Singulars = new List<Inflector.Rule>();
        private static readonly List<string> Uncountables = new List<string>();

        private Inflector()
        {
        }

        static Inflector()
        {
            Inflector.AddPlural("$", "s");
            Inflector.AddPlural("s$", "s");
            Inflector.AddPlural("(ax|test)is$", "$1es");
            Inflector.AddPlural("(octop|vir)us$", "$1i");
            Inflector.AddPlural("(alias|status)$", "$1es");
            Inflector.AddPlural("(bu)s$", "$1ses");
            Inflector.AddPlural("(buffal|tomat)o$", "$1oes");
            Inflector.AddPlural("([ti])um$", "$1a");
            Inflector.AddPlural("sis$", "ses");
            Inflector.AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
            Inflector.AddPlural("(hive)$", "$1s");
            Inflector.AddPlural("([^aeiouy]|qu)y$", "$1ies");
            Inflector.AddPlural("(x|ch|ss|sh)$", "$1es");
            Inflector.AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
            Inflector.AddPlural("([m|l])ouse$", "$1ice");
            Inflector.AddPlural("^(ox)$", "$1en");
            Inflector.AddPlural("(quiz)$", "$1zes");
            Inflector.AddSingular("s$", "");
            Inflector.AddSingular("(n)ews$", "$1ews");
            Inflector.AddSingular("([ti])a$", "$1um");
            Inflector.AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
            Inflector.AddSingular("(^analy)ses$", "$1sis");
            Inflector.AddSingular("([^f])ves$", "$1fe");
            Inflector.AddSingular("(hive)s$", "$1");
            Inflector.AddSingular("(tive)s$", "$1");
            Inflector.AddSingular("([lr])ves$", "$1f");
            Inflector.AddSingular("([^aeiouy]|qu)ies$", "$1y");
            Inflector.AddSingular("(s)eries$", "$1eries");
            Inflector.AddSingular("(m)ovies$", "$1ovie");
            Inflector.AddSingular("(x|ch|ss|sh)es$", "$1");
            Inflector.AddSingular("([m|l])ice$", "$1ouse");
            Inflector.AddSingular("(bus)es$", "$1");
            Inflector.AddSingular("(o)es$", "$1");
            Inflector.AddSingular("(shoe)s$", "$1");
            Inflector.AddSingular("(cris|ax|test)es$", "$1is");
            Inflector.AddSingular("(octop|vir)i$", "$1us");
            Inflector.AddSingular("(alias|status)es$", "$1");
            Inflector.AddSingular("^(ox)en", "$1");
            Inflector.AddSingular("(vert|ind)ices$", "$1ex");
            Inflector.AddSingular("(matr)ices$", "$1ix");
            Inflector.AddSingular("(quiz)zes$", "$1");
            Inflector.AddIrregular("person", "people");
            Inflector.AddIrregular("man", "men");
            Inflector.AddIrregular("child", "children");
            Inflector.AddIrregular("sex", "sexes");
            Inflector.AddIrregular("move", "moves");
            Inflector.AddUncountable("equipment");
            Inflector.AddUncountable("information");
            Inflector.AddUncountable("rice");
            Inflector.AddUncountable("money");
            Inflector.AddUncountable("species");
            Inflector.AddUncountable("series");
            Inflector.AddUncountable("fish");
            Inflector.AddUncountable("sheep");
        }

        public static string Pluralize(string word)
        {
            return Inflector.ApplyRules((IList)Inflector.Plurals, word);
        }

        public static string Singularize(string word)
        {
            return Inflector.ApplyRules((IList)Inflector.Singulars, word);
        }

        public static string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }

        private static void AddIrregular(string singular, string plural)
        {
            Inflector.AddPlural("(" + singular[0].ToString() + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
            Inflector.AddSingular("(" + plural[0].ToString() + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
        }

        private static void AddUncountable(string word)
        {
            Inflector.Uncountables.Add(word.ToLower());
        }

        private static void AddPlural(string rule, string replacement)
        {
            Inflector.Plurals.Add(new Inflector.Rule(rule, replacement));
        }

        private static void AddSingular(string rule, string replacement)
        {
            Inflector.Singulars.Add(new Inflector.Rule(rule, replacement));
        }

        private static string ApplyRules(IList rules, string word)
        {
            string str = word;
            if (!Inflector.Uncountables.Contains(word.ToLower()))
            {
                int index = rules.Count - 1;
                while (index >= 0 && (str = ((Inflector.Rule)rules[index]).Apply(word)) == null)
                    --index;
            }
            return str;
        }

        private class Rule
        {
            private readonly Regex regex;
            private readonly string replacement;

            public Rule(string pattern, string replacement)
            {
                this.regex = new Regex(pattern, RegexOptions.IgnoreCase);
                this.replacement = replacement;
            }

            public string Apply(string word)
            {
                if (!this.regex.IsMatch(word))
                    return (string)null;
                return this.regex.Replace(word, this.replacement);
            }
        }
    }
}
