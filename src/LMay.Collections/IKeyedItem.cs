namespace LMay.Collections
{
    /// <summary>
    /// Represents an item that exposes it's own key through a property.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IKeyedItem<TKey>
    {
        /// <summary>
        /// Gets the key for this item.
        /// </summary>
        TKey Key { get; }
    }
}