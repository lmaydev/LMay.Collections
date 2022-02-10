namespace LMay.Collections;

public class KeyedItemList<TKey, TValue> : KeyedList<TKey, TValue>
    where TKey : notnull
    where TValue : notnull, IKeyedItem<TKey>
{
    public KeyedItemList()
    {
    }

    public KeyedItemList(IEqualityComparer<TKey> comparer) : base(comparer)
    {
    }

    public KeyedItemList(int capacity) : base(capacity)
    {
    }

    public KeyedItemList(IEnumerable<TValue> collection) : base(collection)
    {
    }

    public KeyedItemList(int capacity, IEqualityComparer<TKey>? comparer) : base(capacity, comparer)
    {
    }

    public KeyedItemList(IEnumerable<TValue> collection, IEqualityComparer<TKey>? comparer) : base(collection, comparer)
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.Key;
}