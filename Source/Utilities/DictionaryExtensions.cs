using System;
using System.Collections.Generic;

namespace SharpEngine.Utilities
{
    static class DictionaryExtensions
    {
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> self, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
        }
    }
}
