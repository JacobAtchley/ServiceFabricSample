﻿using System.Linq;

namespace System
{
    public static class Objects
    {
        public static TU CopyPropertiesTo<T, TU>(this T source, TU dest, params string[] propertiesToIgnore)
        {
            var sourceProps = typeof(T)
                .GetProperties()
                .Where(x => x.CanRead)
                .ToList();

            var destProps = typeof(TU)
                .GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (propertiesToIgnore != null && sourceProp.Name.In(propertiesToIgnore))
                {
                    continue;
                }

                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);

                    if (p.CanWrite)
                    {
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }

            }

            return dest;
        }
    }
}
