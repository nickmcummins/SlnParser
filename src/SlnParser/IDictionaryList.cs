using System;
using System.Collections;
using System.Collections.Generic;

namespace SlnParser
{
    public class DictionaryList<TKey, TValue> : ICollection<TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly Func<TValue, TKey> _keyFunc;
        public DictionaryList(Func<TValue, TKey> keyFunc)
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _keyFunc = keyFunc;
        }

        public int Count => _dictionary.Count;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public void Add(TValue item) { _dictionary[_keyFunc(item)] = item; }

        public void Clear() { _dictionary.Clear(); }

        public bool Contains(TValue item) => _dictionary.Values.Contains(item);

        public void CopyTo(TValue[] array, int arrayIndex) { _dictionary.Values.CopyTo(array, arrayIndex); }

        public IEnumerator<TValue> GetEnumerator() => _dictionary.Values.GetEnumerator();

        public bool Remove(TValue item) =>_dictionary.ContainsKey(_keyFunc(item)) && _dictionary.Remove(_keyFunc(item));

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.Values.GetEnumerator();
    }
}
