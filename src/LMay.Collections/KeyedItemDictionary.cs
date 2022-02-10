using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LMay.Collections;

public class KeyedItemDictionary<TKey, TValue> : IKeyedCollection<TKey, TValue>
    where TKey : notnull
    where TValue : IKeyedItem<TKey>
{
    private readonly Dictionary<TKey, TValue> dictionary;

    protected KeyedItemDictionary()
    {
        dictionary = new();
    }

    protected KeyedItemDictionary(IEqualityComparer<TKey>? comparer)
    {
        dictionary = new(comparer);
    }

    protected KeyedItemDictionary(IEnumerable<TValue> collection) : this(collection, null)
    {
    }

    protected KeyedItemDictionary(IEnumerable<TValue> collection, IEqualityComparer<TKey>? comparer)
    {
        this.dictionary = new(comparer);

        foreach (var item in collection)
        {
            Add(item);
        }
    }

    public int Count => dictionary.Count;

    public bool IsReadOnly => false;

    public IEnumerable<TKey> Keys => dictionary.Keys;

    public TValue this[TKey key] => dictionary[key];

    public virtual void Add(TValue item) => dictionary.Add(GetKeyForItem(item), item);

    public virtual IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary() => dictionary;

    public virtual void Clear() => dictionary.Clear();

    public bool Contains(TValue item) => dictionary.ContainsValue(item);

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public void CopyTo(TValue[] array, int arrayIndex) => dictionary.Values.CopyTo(array, arrayIndex);

    public IEnumerator<TValue> GetEnumerator() => dictionary.Values.GetEnumerator();

    public virtual bool Remove(TValue item) => dictionary.Remove(GetKeyForItem(item));

    public virtual bool RemoveByKey(TKey key) => dictionary.Remove(key);

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected TKey GetKeyForItem(TValue item) => item.Key;
}