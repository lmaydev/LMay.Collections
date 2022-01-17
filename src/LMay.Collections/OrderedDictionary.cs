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

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

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
        var comparer = Comparer;

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
        if (ContainsKey(item.Key))
        {
            throw new ArgumentException($"An item with the same key has already been added. Key: {item.Key}");
        }

        list.Insert(index, item);
        dictionary.Add(item.Key, item.Value);
    }

    public bool Remove(TKey key)
    {
        if (!ContainsKey(key))
        {
            return false;
        }

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

        list.RemoveAt(index);
        dictionary.Remove(item.Key);
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
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (arrayIndex + Count >= array.Length)
            {
                throw new ArgumentException("Destination array was not long enough. Check the destination index, length, and the array's lower bounds.", nameof(array));
            }

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
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            public OrderedDictionaryKeyEnumerator(List<KeyValuePair<TKey, TValue>> list)
            {
                enumerator = list.GetEnumerator();
            }

            public TKey Current { get => enumerator.Current.Key; }

            object IEnumerator.Current => Current;

            public void Dispose() => enumerator.Dispose();

            public bool MoveNext() => enumerator.MoveNext();

            public void Reset() => enumerator.Reset();
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
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index was out of range. Must be non-negative and less than the size of the collection.");
            }

            if (arrayIndex + Count >= array.Length)
            {
                throw new ArgumentException("Destination array was not long enough. Check the destination index, length, and the array's lower bounds.", nameof(array));
            }

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
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            public OrderedDictionaryValueEnumerator(List<KeyValuePair<TKey, TValue>> list)
            {
                enumerator = list.GetEnumerator();
            }

            public TValue Current { get => enumerator.Current.Value; }

#pragma warning disable CS8603 // Possible null reference return.

            // Accessing Current before MoveNext or after MoveNext returns false is undefined so default must be allowable here
            object IEnumerator.Current => Current;

#pragma warning restore CS8603 // Possible null reference return.

            public void Dispose() => enumerator.Dispose();

            public bool MoveNext() => enumerator.MoveNext();

            public void Reset() => enumerator.Reset();
        }
    }
}