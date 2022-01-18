// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace System.Collections.Tests
{
    public class OrderedDictionary_List_Generic_Tests_int : OrderedDictionary_List_Generic_Tests<int, int>
    {
        protected override KeyValuePair<int, int> CreateT(int seed)
        {
            Random rand = new Random(seed);
            var value = rand.Next();

            return new(value, value);
        }
    }

    public class OrderedDictionary_List_Generic_Tests_string : OrderedDictionary_List_Generic_Tests<string, string>
    {
        protected override KeyValuePair<string, string> CreateT(int seed)
        {
            int stringLength = seed % 10 + 5;
            Random rand = new Random(seed);
            byte[] bytes = new byte[stringLength];
            rand.NextBytes(bytes);
            var value = Convert.ToBase64String(bytes);

            return new(value, value);
        }
    }
}