namespace LMay.Collections;

public class KeyedItemCollection<TKey, TValue> : KeyedCollection<TKey, TValue>
    where TKey : notnull
    where TValue : IKeyedItem<TKey>
{
    public KeyedItemCollection() : base(new Dictionary<TKey, TValue>())
    {
    }

    public KeyedItemCollection(IEnumerable<TValue> collection) : this(collection, null)
    {
    }

    public KeyedItemCollection(IEqualityComparer<TKey>? comparer) : base(new Dictionary<TKey, TValue>(comparer))
    {
    }

    public KeyedItemCollection(int capacity) : base(new Dictionary<TKey, TValue>(capacity))
    {
    }

    public KeyedItemCollection(IEnumerable<TValue> collection, IEqualityComparer<TKey>? comparer)
        : base(new Dictionary<TKey, TValue>(comparer), collection)
    {
    }

    public KeyedItemCollection(int capacity, IEqualityComparer<TKey>? comparer) : base(new Dictionary<TKey, TValue>(capacity, comparer))
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.Key;
}