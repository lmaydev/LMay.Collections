// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace System.Collections.Tests
{
    /// <summary>
    /// Contains tests that ensure the correctness of the List class.
    /// </summary>
    public abstract partial class OrderedDictionary_List_Generic_Tests<TKey, TValue> : IList_Generic_Tests<KeyValuePair<TKey, TValue>>
    {
        #region Helpers

        public delegate int IndexOfDelegate(IList<KeyValuePair<TKey, TValue>> list, KeyValuePair<TKey, TValue> value);

        public enum IndexOfMethod
        {
            IndexOf_T,
        };

        /// <summary>
        /// MemberData for a Theory to test the IndexOf methods for List. To avoid high code reuse of tests for the 6 IndexOf
        /// methods in List, delegates are used to cover the basic behavioral cases shared by all IndexOf methods. A bool
        /// is used to specify the ordering (front-to-back or back-to-front (e.g. LastIndexOf)) that the IndexOf method
        /// searches in.
        /// </summary>
        public static IEnumerable<object[]> IndexOfTestData()
        {
            foreach (object[] sizes in ValidCollectionSizes())
            {
                int count = (int)sizes[0];
                yield return new object[] { IndexOfMethod.IndexOf_T, count, true };
            }
        }

        private IndexOfDelegate IndexOfDelegateFromType(IndexOfMethod methodType)
        {
            switch (methodType)
            {
                case (IndexOfMethod.IndexOf_T):
                    return ((IList<KeyValuePair<TKey, TValue>> list, KeyValuePair<TKey, TValue> value) => { return list.IndexOf(value); });
                default:
                    throw new Exception("Invalid IndexOfMethod");
            }
        }

        #endregion Helpers

        #region IndexOf

        [Theory]
        [MemberData(nameof(IndexOfTestData))]
        public void IndexOf_NoDuplicates(IndexOfMethod indexOfMethod, int count, bool frontToBackOrder)
        {
            _ = frontToBackOrder;
            IList<KeyValuePair<TKey, TValue>> list = GenericListFactory(count);
            IList<KeyValuePair<TKey, TValue>> expectedList = list.ToList();
            IndexOfDelegate IndexOf = IndexOfDelegateFromType(indexOfMethod);

            Assert.All(Enumerable.Range(0, count), i =>
            {
                Assert.Equal(i, IndexOf(list, expectedList[i]));
            });
        }

        [Theory]
        [MemberData(nameof(IndexOfTestData))]
        public void IndexOf_NonExistingValues(IndexOfMethod indexOfMethod, int count, bool frontToBackOrder)
        {
            _ = frontToBackOrder;
            IList<KeyValuePair<TKey, TValue>> list = GenericListFactory(count);
            IEnumerable<KeyValuePair<TKey, TValue>> nonexistentValues = CreateEnumerable(EnumerableType.List, list, count: count, numberOfMatchingElements: 0, numberOfDuplicateElements: 0);
            IndexOfDelegate IndexOf = IndexOfDelegateFromType(indexOfMethod);

            Assert.All(nonexistentValues, nonexistentValue =>
            {
                Assert.Equal(-1, IndexOf(list, nonexistentValue));
            });
        }

        #endregion IndexOf
    }
}