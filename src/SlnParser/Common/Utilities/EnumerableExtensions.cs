using System.Collections.Generic;

namespace SlnParser.Common.Utilities
{
    public static class EnumerableExtensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair, out TKey key, out TValue value)
        {
            key = keyValuePair.Key;
            value = keyValuePair.Value;
        }

        public static IDictionary<TValue, TKey> AsReverseLookup<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            var invertedDict = new Dictionary<TValue, TKey>();
            foreach (var (k, v) in dict)
                invertedDict[v] = k;
            return invertedDict;
        }
    }
}
