using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LMay.Collections;

public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> dictionary;
    private readonly List<KeyValuePair<TKey, TValue>> list;

    public OrderedDictionary() : this(0)
    {
    }

    public OrderedDictionary(IEqualityComparer<TKey> comparer) : this(0, comparer)
    {
    }

    public OrderedDictionary(int capacity) : this(capacity, null)
    {
    }

    public OrderedDictionary(int capacity, IEqualityComparer<TKey>? comparer)
    {
        list = new(capacity);
        dictionary = new(capacity, comparer);
    }

    public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null)
    {
    }

    public OrderedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
    {
        list = new(collection);
        dictionary = new(collection, comparer);
    }

    public IEqualityComparer<TKey> Comparer => dictionary.Comparer;

    public int Count => list.Count;

    public bool IsReadOnly => false;
    public ICollection<TKey> Keys => new OrderedDictionaryKeyCollection(list, dictionary);

    public ICollection<TValue> Values => new OrderedDictionaryValueCollection(list);

    public KeyValuePair<TKey, TValue> this[int index]
    {
        get => list[index];

        set
        {
            list[index] = value;
            dictionary[value.Key] = value.Value;
        }
    }

    public TValue this[TKey key]
    {
        get => dictionary[key];

        set
        {
            if (dictionary.ContainsKey(key))
            {
                this[IndexOfKey(key)] = new KeyValuePair<TKey, TValue>(key, value);
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
        list.Add(new(key, value));
    }

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public void Clear()
    {
        list.Clear();
        dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.TryGetValue(item.Key, out var value)
                                                             && EqualityComparer<TValue>.Default.Equals(value, item.Value);

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => list.GetEnumerator();

    public int IndexOf(KeyValuePair<TKey, TValue> item) => list.IndexOf(item);

    public int IndexOfKey(TKey key)
    {
        var comparer = dictionary.Comparer;

        for (int i = 0; i < list.Count; i++)
        {
            if (comparer.Equals(list[i].Key, key))
            {
                return i;
            }
        }

        return -1;
    }

    public void Insert(int index, TKey key, TValue value) => Insert(index, new(key, value));

    public void Insert(int index, KeyValuePair<TKey, TValue> item)
    {
        dictionary.Add(item.Key, item.Value);
        list.Insert(index, item);
    }

    public bool Remove(TKey key)
    {
        var index = IndexOfKey(key);

        if (index == -1)
        {
            return false;
        }

        dictionary.Remove(key);
        list.RemoveAt(index);

        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (Contains(item))
        {
            return Remove(item.Key);
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        var item = list[index];

        dictionary.Remove(item.Key);
        list.RemoveAt(index);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private struct OrderedDictionaryKeyCollection : ICollection<TKey>
    {
        private readonly IDictionary<TKey, TValue> dictionary;
        private readonly List<KeyValuePair<TKey, TValue>> list;

        public OrderedDictionaryKeyCollection(List<KeyValuePair<TKey, TValue>> list, Dictionary<TKey, TValue> dictionary)
        {
            this.list = list;
            this.dictionary = dictionary;
        }

        public int Count => list.Count;

        public bool IsReadOnly => true;

        public void Add(TKey item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(TKey item) => dictionary.ContainsKey(item);

        public void CopyTo(TKey[] array, int arrayIndex)
        {
            for (int i = 0; i < list.Count; i++)
            {
                array[arrayIndex + i] = list[i].Key;
            }
        }

        public IEnumerator<TKey> GetEnumerator() => new OrderedDictionaryKeyEnumerator(list);

        public bool Remove(TKey item) => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private struct OrderedDictionaryKeyEnumerator : IEnumerator<TKey>
        {
            private readonly List<KeyValuePair<TKey, TValue>> list;
            private int index = -1;
#nullable disable warnings // Accessing current before calling MoveNext or after MoveNext returns false is undefined. So we need to use default.

            public OrderedDictionaryKeyEnumerator(List<KeyValuePair<TKey, TValue>> list)
            {
                this.list = list;
            }

            public TKey Current { get; private set; } = default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                index++;

                if (index == list.Count)
                {
                    Current = default;
                    return false;
                }

                Current = list[index].Key;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

#nullable enable warnings
        }
    }

    private struct OrderedDictionaryValueCollection : ICollection<TValue>
    {
        private readonly List<KeyValuePair<TKey, TValue>> list;

        public OrderedDictionaryValueCollection(List<KeyValuePair<TKey, TValue>> list)
        {
            this.list = list;
        }

        public int Count => list.Count;

        public bool IsReadOnly => true;

        public void Add(TValue item) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public bool Contains(TValue item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (EqualityComparer<TValue>.Default.Equals(item, list[i].Value))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = 0; i < list.Count; i++)
            {
                array[arrayIndex + i] = list[i].Value;
            }
        }

        public IEnumerator<TValue> GetEnumerator() => new OrderedDictionaryValueEnumerator(list);

        public bool Remove(TValue item) => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private struct OrderedDictionaryValueEnumerator : IEnumerator<TValue>
        {
            private readonly List<KeyValuePair<TKey, TValue>> list;
            private int index = -1;
#nullable disable warnings // Accessing current before calling MoveNext or after MoveNext returns false is undefined. So we need to use default.

            public OrderedDictionaryValueEnumerator(List<KeyValuePair<TKey, TValue>> list)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            {
                this.list = list;
            }

            public TValue Current { get; private set; } = default;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                index++;

                if (index == list.Count)
                {
                    Current = default;
                    return false;
                }

                Current = list[index].Value;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

#nullable enable warnings
        }
    }
}