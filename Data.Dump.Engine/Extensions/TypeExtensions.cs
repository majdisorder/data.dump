using Data.Dump.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Data.Dump.Extensions
{
    public static class TypeExtensions
    {
        private static Dictionary<Type, string> _cachedDefaultTypeCollectionNames = new Dictionary<Type, string>();
        private static readonly Dictionary<Type, Type> CachedEnumerableTypeArguments = new Dictionary<Type, Type>();

        public static bool IsEnumerable(this Type me)
        {
            return typeof(IEnumerable).IsAssignableFrom(me);
        }

        public static Type GetGenericEnumerableTypeArgument(this Type type)
        {
            if (CachedEnumerableTypeArguments.TryGetValue(type, out var argType))
                return argType;

            if (type.IsGenericType)
            {
                if (type.GenericTypeArguments.Length == 1)
                {
                    CachedEnumerableTypeArguments.Add(type, type.GenericTypeArguments[0]);
                    return type.GenericTypeArguments[0];
                }

                if (type.IsAssignableToGenericTypeDefinition(typeof(IDictionary<,>)))
                {
                    argType = type.GetInterfaces()
                        .Select(x => x.IsGenericType ? x.GenericTypeArguments.FirstOrDefault() : null)
                        .FirstOrDefault(x => x?.IsAssignableToGenericTypeDefinition(typeof(KeyValuePair<,>)) ?? false);

                    if (argType != null)
                    {
                        CachedEnumerableTypeArguments.Add(type, argType);
                        return argType;
                    }
                }
            }

            throw new InvalidTypeException(nameof(type), type, typeof(IEnumerable<>));
        }

        public static bool IsAssignableTo(this Type type, Type otherType)
        {
            var typeInfo = type.GetTypeInfo();
            var otherTypeInfo = otherType.GetTypeInfo();

            if (otherTypeInfo.IsGenericTypeDefinition)
            {
                if (typeInfo.IsGenericTypeDefinition)
                {
                    return typeInfo.Equals(otherTypeInfo);
                }

                return typeInfo.IsAssignableToGenericTypeDefinition(otherTypeInfo);
            }

            return otherTypeInfo.IsAssignableFrom(typeInfo);
        }

        private static bool IsAssignableToGenericTypeDefinition(this Type me, Type type)
        {
            while (!type.IsAssignableFrom(me))
            {
                if (me == typeof(object) || me == null)
                {
                    return false;
                }
                if (me.IsGenericType && !me.IsGenericTypeDefinition)
                {
                    me = me.GetGenericTypeDefinition();
                }
                else
                {
                    me = me.BaseType;
                }
            }

            return true;
        }

        //from Raven.Client.Documents.Conventions.DocumentConventions.DefaultGetCollectionName
        public static string GetReadableName(this Type me)
        {
            if (_cachedDefaultTypeCollectionNames.TryGetValue(me, out var str1))
                return str1;
            if (me.Name.Contains("<>"))
                return (string)null;
            if (me.GetTypeInfo().IsInterface)
                throw new InvalidOperationException("Cannot find collection name for interface " + me.FullName + ", only concrete classes are supported.");
            if (me.GetTypeInfo().IsAbstract)
                throw new InvalidOperationException("Cannot find collection name for abstract class " + me.FullName + ", only concrete class are supported.");
            string str2;
            if (me.GetTypeInfo().IsGenericType)
            {
                string str3 = me.GetGenericTypeDefinition().Name;
                if (str3.Contains<char>('`'))
                    str3 = str3.Substring(0, str3.IndexOf('`'));
                StringBuilder stringBuilder = new StringBuilder(Inflector.Pluralize(str3));
                foreach (var genericArgument in me.GetGenericArguments())
                    stringBuilder.Append("Of").Append(genericArgument.GetReadableName());
                str2 = stringBuilder.ToString();
            }
            else
                str2 = !(me == typeof(object)) ? Inflector.Pluralize(me.Name) : "@all_docs";

            Dictionary<Type, string> dictionary = new Dictionary<Type, string>((IDictionary<Type, string>)_cachedDefaultTypeCollectionNames)
            {
                [me] = str2
            };
            _cachedDefaultTypeCollectionNames = dictionary;
            return str2;
        }
    }
}
