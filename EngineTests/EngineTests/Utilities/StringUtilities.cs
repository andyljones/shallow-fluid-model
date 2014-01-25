using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EngineTests.Utilities
{
    public static class StringUtilities
    {
        public static string CollectionToString<T>(IEnumerable<T> collection)
        {
            var itemStrings = collection.Select(item => item.ToString()).ToArray();
            var enumerableString = String.Join(", ", itemStrings);

            return enumerableString;
        }
    }
}
