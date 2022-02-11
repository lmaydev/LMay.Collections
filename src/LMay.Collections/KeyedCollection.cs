using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace LMay.Collections;

public abstract class KeyedCollection<TKey, TValue> : IKeyedCollection<TKey, TValue>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> dictionary;

    protected KeyedCollection(IDictionary<TKey, TValue> backingDictionary)
    {
        dictionary = backingDictionary;
    }

    public int Count => dictionary.Count;

    public bool IsReadOnly => false;

    public IEnumerable<TKey> Keys => dictionary.Keys;

    public TValue this[TKey key] => dictionary[key];

    public virtual void Add(TValue item) => dictionary.Add(GetKeyForItem(item), item);

    public virtual IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary() => new ReadOnlyDictionaryWrapper<TKey, TValue>(dictionary);

    public virtual void Clear() => dictionary.Clear();

    public bool Contains(TValue item) => dictionary.Values.Contains(item);

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public void CopyTo(TValue[] array, int arrayIndex) => dictionary.Values.CopyTo(array, arrayIndex);

    public IEnumerator<TValue> GetEnumerator() => dictionary.Values.GetEnumerator();

    public virtual bool Remove(TValue item) => dictionary.Remove(GetKeyForItem(item));

    public virtual bool RemoveByKey(TKey key) => dictionary.Remove(key);

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract TKey GetKeyForItem(TValue item);
}