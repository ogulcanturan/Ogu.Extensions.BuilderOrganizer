#if NETSTANDARD2_0
using System;
using System.Collections.Generic;

namespace Ogu.Extensions.BuilderOrganizer
{

    internal static class Extensions
    {
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.ContainsKey(key))
                return false;

            dictionary[key] = value;

            return true;
        }
    }
}
#endif