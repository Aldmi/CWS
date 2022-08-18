using System;
using System.Collections.Concurrent;


namespace Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAddExt<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, Func<TValue> valueFactory) where TKey : notnull
        {
            if (dict.TryGetValue(key, out var resultValue))
                return resultValue;

            var v = valueFactory();
            dict.TryAdd(key, v);
            return v;
        }
    }
}