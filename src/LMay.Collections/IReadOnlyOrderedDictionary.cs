namespace System.Collections.Generic;

public interface IReadOnlyOrderedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    IEqualityComparer<TKey> Comparer { get; }

    int IndexOfKey(TKey key);
}