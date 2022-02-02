using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    public interface IKeyedList<TKey, TValue> : IList<TValue> where TKey : notnull
    {
        IEnumerable<TKey> Keys { get; }
        TValue this[TKey key] { get; }

        bool ContainsKey(TKey key);

        bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
    }
}