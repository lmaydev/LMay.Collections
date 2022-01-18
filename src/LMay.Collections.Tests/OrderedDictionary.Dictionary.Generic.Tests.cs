// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Tests;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
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

        protected override IDictionary<TKey, TValue> GenericIDictionaryFactory() => new OrderedDictionary<TKey, TValue>();

        protected override IDictionary<TKey, TValue> GenericIDictionaryFactory(IEqualityComparer<TKey> comparer) => new OrderedDictionary<TKey, TValue>(comparer);

        #endregion IDictionary<TKey, TValue Helper Methods

        #region Constructors

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_IDictionary(int count)
        {
            IDictionary<TKey, TValue> source = GenericIDictionaryFactory(count);
            IDictionary<TKey, TValue> copied = new Dictionary<TKey, TValue>(source);
            Assert.Equal(source, copied);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_IDictionary_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();
            IDictionary<TKey, TValue> source = GenericIDictionaryFactory(count);
            Dictionary<TKey, TValue> copied = new Dictionary<TKey, TValue>(source, comparer);
            Assert.Equal(source, copied);
            Assert.Equal(comparer, copied.Comparer);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();
            IDictionary<TKey, TValue> source = GenericIDictionaryFactory(count);
            Dictionary<TKey, TValue> copied = new Dictionary<TKey, TValue>(source, comparer);
            Assert.Equal(source, copied);
            Assert.Equal(comparer, copied.Comparer);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_int(int count)
        {
            IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(count);
            Assert.Equal(0, dictionary.Count);
        }

        [Theory]
        [MemberData(nameof(ValidCollectionSizes))]
        public void Dictionary_Generic_Constructor_int_IEqualityComparer(int count)
        {
            IEqualityComparer<TKey> comparer = GetKeyIEqualityComparer();
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(count, comparer);
            Assert.Empty(dictionary);
            Assert.Equal(comparer, dictionary.Comparer);
        }

        #endregion Constructors

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

        #region Non-randomized comparers

        [Fact]
        public void Dictionary_Comparer_NonRandomizedStringComparers()
        {
            RunTest(null);
            RunTest(EqualityComparer<string>.Default);
            RunTest(StringComparer.Ordinal);
            RunTest(StringComparer.OrdinalIgnoreCase);
            RunTest(StringComparer.InvariantCulture);
            RunTest(StringComparer.InvariantCultureIgnoreCase);
            RunTest(StringComparer.Create(CultureInfo.InvariantCulture, ignoreCase: false));
            RunTest(StringComparer.Create(CultureInfo.InvariantCulture, ignoreCase: true));

            void RunTest(IEqualityComparer<string> comparer)
            {
                // First, instantiate the dictionary and check its Comparer property

                Dictionary<string, object> dict = new Dictionary<string, object>(comparer);
                object expected = comparer ?? EqualityComparer<string>.Default;

                Assert.Same(expected, dict.Comparer);

                // Then pretend to serialize the dictionary and check the stored Comparer instance

                SerializationInfo si = new SerializationInfo(typeof(Dictionary<string, object>), new FormatterConverter());
                dict.GetObjectData(si, new StreamingContext(StreamingContextStates.All));

                Assert.Same(expected, si.GetValue("Comparer", typeof(IEqualityComparer<string>)));
            }
        }

        #endregion Non-randomized comparers
    }
}