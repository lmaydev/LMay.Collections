namespace LMay.Collections
{
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

        public SortedKeyedItemCollection(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public SortedKeyedItemCollection(IDictionary<TKey, TValue> dictionary, IComparer<TKey>? comparer) : base(dictionary, comparer)
        {
        }

        protected override TKey GetKeyForItem(TValue item) => item.Key;
    }
}