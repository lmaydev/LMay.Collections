using System;
using System.Collections.Generic;
using System.Collections.Tests;

namespace LMay.Collections.Tests
{
    public struct IntPair : IKeyedItem<int>
    {
        public IntPair(int value)
        {
            Value = value;
        }

        public int Key => Value;

        public int Value { get; set; }
    }

    public struct StringPair : IKeyedItem<string>
    {
        public StringPair(string value)
        {
            Value = value;
        }

        public string Key => Value;

        public string Value { get; }
    }

    public class KeyedList_List_Generic_Tests_IntPair : IList_Generic_Tests<IntPair>
    {
        protected override bool DuplicateValuesAllowed => false;
        protected override Type ICollection_Generic_CopyTo_IndexLargerThanArrayCount_ThrowType => typeof(ArgumentOutOfRangeException);

        protected override IntPair CreateT(int seed)
        {
            var random = new Random(seed);

            return new(random.Next());
        }

        protected override IList<IntPair> GenericIListFactory() => new KeyedItemList<int, IntPair>();
    }

    public class KeyedList_List_Generic_Tests_StringPair : IList_Generic_Tests<StringPair>
    {
        protected override bool DefaultValueAllowed => false;

        protected override bool DefaultValueWhenNotAllowed_Throws => false;

        protected override bool DuplicateValuesAllowed => false;

        protected override Type ICollection_Generic_CopyTo_IndexLargerThanArrayCount_ThrowType => typeof(ArgumentOutOfRangeException);

        public override void ICollection_Generic_Contains_DefaultValueWhenNotAllowed(int count)
        {
            // Not applicable
        }

        public override void ICollection_Generic_Remove_DefaultValueWhenNotAllowed(int count)
        {
            // Not applicable
        }

        public override void IList_Generic_IndexOf_DefaultValueNotContainedInList(int count)
        {
            // Not applicable
        }

        public override void IList_Generic_ItemSet_FirstItemToDefaultValue(int count)
        {
            // Not applicable
        }

        protected override StringPair CreateT(int seed)
        {
            int stringLength = seed % 10 + 5;
            Random rand = new Random(seed);
            byte[] bytes = new byte[stringLength];
            rand.NextBytes(bytes);
            return new(Convert.ToBase64String(bytes));
        }

        protected override IList<StringPair> GenericIListFactory() => new KeyedItemList<string, StringPair>();
    }
}