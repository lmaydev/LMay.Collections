// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LMay.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Tests;
using System.Diagnostics;
using Xunit;

namespace LMay.Collections.Tests
{
    public class OrderedDictionary_Dictionary_Generic_Tests_Values : ICollection_Generic_Tests<string>
    {
        protected override bool DefaultValueAllowed => true;
        protected override bool DuplicateValuesAllowed => true;
        protected override Type ICollection_Generic_CopyTo_IndexLargerThanArrayCount_ThrowType => typeof(ArgumentOutOfRangeException);
        protected override bool IsReadOnly => true;

        protected override string CreateT(int seed)
        {
            int stringLength = seed % 10 + 5;
            Random rand = new Random(seed);
            byte[] bytes = new byte[stringLength];
            rand.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        protected override ICollection<string> GenericICollectionFactory()
        {
            return new OrderedDictionary<string, string>().Values;
        }

        protected override ICollection<string> GenericICollectionFactory(int count)
        {
            var list = new OrderedDictionary<string, string>();
            int seed = 13453;
            for (int i = 0; i < count; i++)
                list.Add(CreateT(seed++), CreateT(seed++));
            return list.Values;
        }

        protected override IEnumerable<ModifyEnumerable> GetModifyEnumerables(ModifyOperation operations) => new List<ModifyEnumerable>();
    }
}