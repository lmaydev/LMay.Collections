namespace LMay.Collections;

public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    IEqualityComparer<TKey> Comparer { get; }

    int IndexOfKey(TKey key);

    void Insert(int index, TKey key, TValue value);
}