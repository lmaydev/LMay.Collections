// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Tests;
using System.Linq;
using Xunit;

namespace LMay.Collections.Tests
{
    /// <summary>
    /// Contains tests that ensure the correctness of the Dictionary class.
    /// </summary>
    public abstract class OrderedDictionary_Generic_Tests<TKey, TValue> : IDictionary_Generic_Tests<TKey, TValue>
    {
        protected override ModifyOperation ModifyEnumeratorAllowed => ModifyOperation.None;
        protected override ModifyOperation ModifyEnumeratorThrows => ModifyOperation.Overwrite | ModifyOperation.Remove | ModifyOperation.Clear | ModifyOperation.Add | ModifyOperation.Insert;

        #region IDictionary<TKey, TValue Helper Methods

        protected override Type ICollection_Generic_CopyTo_IndexLargerThanArrayCount_ThrowType => typeof(ArgumentException);

        protected override IDictionary<TKey, TValue> GenericIDictionaryFactory() => IOrderedDictionaryFactory();

        protected override IDictionary<TKey, TValue> GenericIDictionaryFactory(IEqualityComparer<TKey> comparer)
            => IOrderedDictionaryFactory(comparer);

        protected virtual IOrderedDictionary<TKey, TValue> IOrderedDictionaryFactory(int count)
        {
            var list = new List<KeyValuePair<TKey, TValue>>();

            for (int i = 0; i < count; i++)
            {
                var seed = count + i;

                list.Add(CreateT(seed));
            }

            return new OrderedDictionary<TKey, TValue>(list);
        }

        protected virtual IOrderedDictionary<TKey, TValue> IOrderedDictionaryFactory() => new OrderedDictionary<TKey, TValue>();

        protected virtual IOrderedDictionary<TKey, TValue> IOrderedDictionaryFactory(IEqualityComparer<TKey> comparer)
            => new OrderedDictionary<TKey, TValue>(comparer);

        #endregion IDictionary<TKey, TValue Helper Methods

        #region Constructors

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_IDictionary(int count)
        {
            IDictionary<TKey, TValue> source = GenericIDictionaryFactory(count);
            IDictionary<TKey, TValue> copied = new OrderedDictionary<TKey, TValue>(source);
            Assert.Equal(source, copied);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_IDictionary_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();
            IDictionary<TKey, TValue> source = GenericIDictionaryFactory(count);
            IOrderedDictionary<TKey, TValue> copied = new OrderedDictionary<TKey, TValue>(source, comparer);
            Assert.Equal(source, copied);
            Assert.Equal(comparer, copied.Comparer);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();
            IDictionary<TKey, TValue> source = GenericIDictionaryFactory(count);
            IOrderedDictionary<TKey, TValue> copied = new OrderedDictionary<TKey, TValue>(source, comparer);
            Assert.Equal(source, copied);
            Assert.Equal(comparer, copied.Comparer);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_int(int count)
        {
            IDictionary<TKey, TValue> dictionary = new OrderedDictionary<TKey, TValue>(count);
            Assert.Equal(0, dictionary.Count);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_int_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();
            IOrderedDictionary<TKey, TValue> dictionary = new OrderedDictionary<TKey, TValue>(count, comparer);
            Assert.Empty(dictionary);
            Assert.Equal(comparer, dictionary.Comparer);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void OrderedDictionary_Generic_Constructor_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();

            IOrderedDictionary<TKey, TValue> dictionary = new OrderedDictionary<TKey, TValue>(comparer);

            Assert.Equal(comparer, dictionary.Comparer);
        }

        #endregion Constructors

        #region Remove(TKey)

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_RemoveKey_DefaultKeyContainedInDictionary(int count)
        {
            if (DefaultValueAllowed)
            {
                var dictionary = GenericIDictionaryFactory(count);
                TKey missingKey = default;
                TValue value;

                dictionary.TryAdd(missingKey, default);
                Assert.True(dictionary.Remove(missingKey, out value));
            }
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_RemoveKey_DefaultKeyNotContainedInDictionary(int count)
        {
            var dictionary = GenericIDictionaryFactory(count);
            TValue outValue;

            if (DefaultValueAllowed)
            {
                TKey missingKey = default;
                while (dictionary.ContainsKey(missingKey))
                    dictionary.Remove(missingKey);
                Assert.False(dictionary.Remove(missingKey, out outValue));
                Assert.Equal(default, outValue);
            }
            else
            {
                TValue initValue = CreateTValue(count);
                outValue = initValue;
                Assert.Throws<ArgumentNullException>(() => dictionary.Remove(default, out outValue));
                Assert.Equal(initValue, outValue);
            }
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_RemoveKey_ValidKeyContainedInDictionary(int count)
        {
            var dictionary = GenericIDictionaryFactory(count);
            TKey missingKey = GetNewKey(dictionary);
            TValue outValue;
            TValue inValue = CreateTValue(count);

            dictionary.Add(missingKey, inValue);
            Assert.True(dictionary.Remove(missingKey, out outValue));
            Assert.Equal(count, dictionary.Count);
            Assert.Equal(inValue, outValue);
            Assert.False(dictionary.TryGetValue(missingKey, out outValue));
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_RemoveKey_ValidKeyNotContainedInDictionary(int count)
        {
            var dictionary = GenericIDictionaryFactory(count);
            TValue value;
            TKey missingKey = GetNewKey(dictionary);

            Assert.False(dictionary.Remove(missingKey, out value));
            Assert.Equal(count, dictionary.Count);
            Assert.Equal(default, value);
        }

        #endregion Remove(TKey)

        #region IndexOfKey

        [Theory, MemberData(nameof(ValidCollectionSizesAndIndex))]
        public void IOrderedDictionary_IndexOfKey(int count, int index)
        {
            IOrderedDictionary<TKey, TValue> dictionary = IOrderedDictionaryFactory(count);

            var key = dictionary.Keys.Skip(index).First();

            var actual = dictionary.IndexOfKey(key);

            Assert.Equal(index, actual);
        }

        [Theory, MemberData(nameof(ValidPositiveCollectionSizes))]
        public void IOrderedDictionary_IndexOfKey_Invalid(int count)
        {
            IOrderedDictionary<TKey, TValue> dictionary = IOrderedDictionaryFactory(count);

            var key = GetNewKey(dictionary);

            var actual = dictionary.IndexOfKey(key);

            Assert.Equal(-1, actual);
        }

        #endregion IndexOfKey

        #region Insert Key/Value

        [Theory, MemberData(nameof(ValidCollectionSizesAndIndex))]
        public void IOrderedDictionary_Insert_Key_Value(int count, int index)
        {
            IOrderedDictionary<TKey, TValue> dictionary = IOrderedDictionaryFactory(count);

            var key = GetNewKey(dictionary);

            var value = CreateTValue(count);

            dictionary.Insert(index, key, value);

            var item = dictionary[index];

            Assert.Equal(key, item.Key);
            Assert.Equal(value, item.Value);
        }

        [Theory, MemberData(nameof(CollectionSizesAndInvalidIndex))]
        public void IOrderedDictionary_Insert_Key_Value_Invalid_Index(int count, int index)
        {
            IOrderedDictionary<TKey, TValue> dictionary = IOrderedDictionaryFactory(count);

            var key = GetNewKey(dictionary);

            var value = CreateTValue(count);

            Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.Insert(index, key, value));
        }

        #endregion Insert Key/Value

        #region IReadOnlyDictionary<TKey, TValue>.Keys

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void IReadOnlyDictionary_Generic_Keys_ContainsAllCorrectKeys(int count)
        {
            IDictionary<TKey, TValue> dictionary = GenericIDictionaryFactory(count);
            IEnumerable<TKey> expected = dictionary.Select((pair) => pair.Key);
            IEnumerable<TKey> keys = ((IReadOnlyDictionary<TKey, TValue>)dictionary).Keys;
            Assert.True(expected.SequenceEqual(keys));
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void IReadOnlyDictionary_Generic_Values_ContainsAllCorrectValues(int count)
        {
            IDictionary<TKey, TValue> dictionary = GenericIDictionaryFactory(count);
            IEnumerable<TValue> expected = dictionary.Select((pair) => pair.Value);
            IEnumerable<TValue> values = ((IReadOnlyDictionary<TKey, TValue>)dictionary).Values;
            Assert.True(expected.SequenceEqual(values));
        }

        #endregion IReadOnlyDictionary<TKey, TValue>.Keys

        public static IEnumerable<object[]> CollectionSizesAndInvalidIndex()
        {
            // invalid
            yield return new object[] { 1, -1 };
            // out of range
            yield return new object[] { 0, 1 };
            yield return new object[] { 75, 100 };
        }

        public static IEnumerable<object[]> ValidCollectionSizesAndIndex()
        {
            yield return new object[] { 1, 0 };
            yield return new object[] { 75, 50 };
        }
    }
}