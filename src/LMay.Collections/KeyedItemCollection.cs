namespace LMay.Collections;

public class KeyedItemCollection<TKey, TValue> : KeyedCollection<TKey, TValue>
    where TKey : notnull
    where TValue : IKeyedItem<TKey>
{
    public KeyedItemCollection() : base(new Dictionary<TKey, TValue>())
    {
    }

    public KeyedItemCollection(IDictionary<TKey, TValue> dictionary) : base(new Dictionary<TKey, TValue>(dictionary))
    {
    }

    public KeyedItemCollection(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(new Dictionary<TKey, TValue>(collection))
    {
    }

    public KeyedItemCollection(IEqualityComparer<TKey>? comparer) : base(new Dictionary<TKey, TValue>(comparer))
    {
    }

    public KeyedItemCollection(int capacity) : base(new Dictionary<TKey, TValue>(capacity))
    {
    }

    public KeyedItemCollection(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer) : base(new Dictionary<TKey, TValue>(dictionary, comparer))
    {
    }

    public KeyedItemCollection(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) : base(new Dictionary<TKey, TValue>(collection, comparer))
    {
    }

    public KeyedItemCollection(int capacity, IEqualityComparer<TKey>? comparer) : base(new Dictionary<TKey, TValue>(capacity, comparer))
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.Key;
}