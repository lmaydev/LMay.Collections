// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using LMay.Collections;
using System.Collections.Generic;
using Xunit;

namespace System.Collections.Tests
{
    /// <summary>
    /// Contains tests that ensure the correctness of the List class.
    /// </summary>
    public abstract partial class OrderedDictionary_List_Generic_Tests<TKey, TValue> : IList_Generic_Tests<KeyValuePair<TKey, TValue>>
    {
        protected override bool DefaultValueAllowed => false;
        protected override bool DefaultValueWhenNotAllowed_Throws => true;
        protected override bool DuplicateValuesAllowed => false;

        public override void ICollection_Generic_Contains_DefaultValueWhenNotAllowed(int count)
        {
            // Doesnt apply to OrderedDictionary
        }

        public override void ICollection_Generic_Remove_DefaultValueWhenNotAllowed(int count)
        {
            // Doesnt apply to OrderedDictionary
        }

        public override void IList_Generic_IndexOf_DefaultValueNotContainedInList(int count)
        {
            // Doesnt apply to OrderedDictionary
        }

        public override void IList_Generic_ItemSet_FirstItemToDefaultValue(int count)
        {
            // Doesnt apply to OrderedDictionary
        }

        #region IList<T> Helper Methods

        protected override IList<KeyValuePair<TKey, TValue>> GenericIListFactory()
        {
            return GenericListFactory();
        }

        protected override IList<KeyValuePair<TKey, TValue>> GenericIListFactory(int count)
        {
            return GenericListFactory(count);
        }

        #endregion IList<T> Helper Methods

        #region List<T> Helper Methods

        protected virtual IList<KeyValuePair<TKey, TValue>> GenericListFactory()
        {
            return new OrderedDictionary<TKey, TValue>();
        }

        protected virtual IList<KeyValuePair<TKey, TValue>> GenericListFactory(int count)
        {
            var fixture = new Fixture();

            var items = fixture.CreateMany<KeyValuePair<TKey, TValue>>(count);

            return new OrderedDictionary<TKey, TValue>(items);
        }

        protected void VerifyList(IList<KeyValuePair<TKey, TValue>> list, IList<KeyValuePair<TKey, TValue>> expectedItems)
        {
            Assert.Equal(expectedItems.Count, list.Count);

            //Only verify the indexer. List should be in a good enough state that we
            //do not have to verify consistency with any other method.
            for (int i = 0; i < list.Count; ++i)
            {
                Assert.True(list[i].Equals(expectedItems[i]));
            }
        }

        #endregion List<T> Helper Methods
    }
}