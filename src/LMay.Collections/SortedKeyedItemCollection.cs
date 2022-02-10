namespace LMay.Collections;

public class SortedKeyedItemCollection<TKey, TValue> : SortedKeyedCollection<TKey, TValue>
    where TKey : notnull
    where TValue : notnull, IKeyedItem<TKey>
{
    public SortedKeyedItemCollection()
    {
    }

    public SortedKeyedItemCollection(IComparer<TKey>? comparer) : base(comparer)
    {
    }

    public SortedKeyedItemCollection(IEnumerable<TValue> collection) : base(collection)
    {
    }

    public SortedKeyedItemCollection(IEnumerable<TValue> collection, IComparer<TKey>? comparer) : base(collection, comparer)
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.Key;
}