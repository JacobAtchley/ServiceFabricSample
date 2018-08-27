using System;

namespace Fabric.Web
{
    public static class WebExtension
    {
        public static TKey ParseKey<TKey>(this string source)
        {
            var tKeyType = typeof(TKey);

            if (tKeyType == typeof(long))
            {
                if (long.TryParse(source, out var l))
                {
                    return (TKey)Convert.ChangeType(l, typeof(long));
                }
            }
            else if (tKeyType == typeof(int))
            {
                if (int.TryParse(source, out var i))
                {
                    return (TKey)Convert.ChangeType(i, typeof(int));
                }

            }
            else if (tKeyType == typeof(Guid))
            {
                if (Guid.TryParse(source, out var g))
                {
                    return (TKey)Convert.ChangeType(g, typeof(Guid));
                }

            }
            else if (tKeyType == typeof(string))
            {
                return (TKey)Convert.ChangeType(source, typeof(string));
            }

            throw new Exception("Could not parse key");
        }
    }
}
