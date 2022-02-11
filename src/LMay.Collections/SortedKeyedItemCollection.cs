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

    public SortedKeyedItemCollection(IDictionary<TKey, TValue> dictionary) : base(new SortedDictionary<TKey, TValue>(dictionary))
    {
    }

    public SortedKeyedItemCollection(IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer)
        : base(new SortedDictionary<TKey, TValue>(dictionary, comparer))
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.Key;
}