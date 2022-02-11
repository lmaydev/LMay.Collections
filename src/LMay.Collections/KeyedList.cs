namespace LMay.Collections;

public abstract class KeyedList<TKey, TValue> : KeyedCollection<TKey, TValue>, IList<TValue> where TKey : notnull
{
    protected KeyedList() : this(0)
    {
    }

    protected KeyedList(IEqualityComparer<TKey> comparer) : this(0, comparer)
    {
    }

    protected KeyedList(int capacity) : this(capacity, null)
    {
    }

    protected KeyedList(int capacity, IEqualityComparer<TKey>? comparer) :
        base(new OrderedDictionary<TKey, TValue>(capacity, comparer))
    {
    }

    protected KeyedList(IEnumerable<TValue> collection) : this(collection, null)
    {
    }

    protected KeyedList(IEnumerable<TValue> collection, IEqualityComparer<TKey>? comparer) :
        base(new OrderedDictionary<TKey, TValue>(comparer), collection)
    {
    }

    protected OrderedDictionary<TKey, TValue> OrderedDictionary => (OrderedDictionary<TKey, TValue>)dictionary;

    public virtual TValue this[int index]
    {
        get => OrderedDictionary[index].Value;
        set => OrderedDictionary[index] = GetPair(value);
    }

    public int IndexOf(TValue item) => OrderedDictionary.IndexOf(GetPair(item));

    public virtual void Insert(int index, TValue item) => OrderedDictionary.Insert(index, GetPair(item));

    public virtual void RemoveAt(int index) => OrderedDictionary.RemoveAt(index);
}