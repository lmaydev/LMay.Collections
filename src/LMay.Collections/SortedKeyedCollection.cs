using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LMay.Collections
{
    public abstract class SortedKeyedCollection<TKey, TValue> : ICollection<TValue>, IKeyedCollection<TKey, TValue>
        where TKey : notnull
    {
        private readonly SortedDictionary<TKey, TValue> dictionary;

        protected SortedKeyedCollection()
        {
            dictionary = new();
        }

        protected SortedKeyedCollection(IComparer<TKey>? comparer)
        {
            dictionary = new(comparer);
        }

        protected SortedKeyedCollection(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = new(dictionary);
        }

        protected SortedKeyedCollection(IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer)
        {
            this.dictionary = new(dictionary, comparer);
        }

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public IEnumerable<TKey> Keys => dictionary.Keys;

        public TValue this[TKey key] => dictionary[key];

        public void Add(TValue item) => dictionary.Add(GetKeyForItem(item), item);

        public void Clear() => dictionary.Clear();

        public bool Contains(TValue item) => dictionary.ContainsValue(item);

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public void CopyTo(TValue[] array, int arrayIndex) => dictionary.Values.CopyTo(array, arrayIndex);

        public IEnumerator<TValue> GetEnumerator() => dictionary.Values.GetEnumerator();

        public bool Remove(TValue item) => dictionary.Remove(GetKeyForItem(item));

        public bool RemoveByKey(TKey key) => dictionary.Remove(key);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected abstract TKey GetKeyForItem(TValue item);
    }
}