using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public abstract class KeyedList<TKey, TValue> : IList<TValue>, IKeyedCollection<TKey, TValue> where TKey : notnull
{
    private readonly OrderedDictionary<TKey, TValue> dictionary;

    protected KeyedList() : this(0)
    {
    }

    protected KeyedList(IEqualityComparer<TKey> comparer) : this(0, comparer)
    {
    }

    protected KeyedList(int capacity) : this(capacity, null)
    {
    }

    protected KeyedList(int capacity, IEqualityComparer<TKey>? comparer)
    {
        dictionary = new(capacity, comparer);
    }

    protected KeyedList(IEnumerable<TValue> collection) : this(collection, null)
    {
    }

    protected KeyedList(IEnumerable<TValue> collection, IEqualityComparer<TKey>? comparer)
    {
        dictionary = new(comparer);

        foreach (var item in collection)
        {
            dictionary[GetKeyForItem(item)] = item;
        }
    }

    public int Count => dictionary.Count;

    public bool IsReadOnly => false;

    public IEnumerable<TKey> Keys => dictionary.Keys;

    public TValue this[TKey key] => dictionary[key];

    public TValue this[int index]
    {
        get => dictionary[index].Value;
        set => dictionary[index] = GetPair(value);
    }

    public void Add(TValue item) => dictionary.Add(GetPair(item));

    public void Clear() => dictionary.Clear();

    public bool Contains(TValue item) => dictionary.Contains(GetPair(item));

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public void CopyTo(TValue[] array, int arrayIndex) => dictionary.Values.CopyTo(array, arrayIndex);

    public IEnumerator<TValue> GetEnumerator() => dictionary.Values.GetEnumerator();

    public int IndexOf(TValue item) => dictionary.IndexOf(GetPair(item));

    public void Insert(int index, TValue item) => dictionary.Insert(index, GetPair(item));

    public bool Remove(TValue item) => dictionary.Remove(GetPair(item));

    public void RemoveAt(int index) => dictionary.RemoveAt(index);

    public bool RemoveByKey(TKey key)
    {
        if (!ContainsKey(key))
        {
            return false;
        }

        var index = IndexOf(dictionary[key]);

        RemoveAt(index);

        return true;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract TKey GetKeyForItem(TValue item);

    protected KeyValuePair<TKey, TValue> GetPair(TValue item) => new(GetKeyForItem(item), item);
}