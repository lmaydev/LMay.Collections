namespace LMay.Collections;

public class SortedKeyedItemCollection<TKey, TValue> : KeyedCollection<TKey, TValue>
    where TKey : notnull
    where TValue : notnull, IKeyedItem<TKey>
{
    public SortedKeyedItemCollection() : base(new SortedDictionary<TKey, TValue>())
    {
    }

    public SortedKeyedItemCollection(IComparer<TKey>? comparer) : base(new SortedDictionary<TKey, TValue>(comparer))
    {
    }

    public SortedKeyedItemCollection(IEnumerable<TValue> collection) : this(collection, null)
    {
    }

    public SortedKeyedItemCollection(IEnumerable<TValue> collection, IComparer<TKey>? comparer)
        : base(new SortedDictionary<TKey, TValue>(comparer), collection)
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.Key;
}