using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Utilities
{
    public static class CyclicIndexingExtensions
    {
        public static T AtCyclicIndex<T>(this IList<T> list, int i)
        {
            int max = list.Count;
            int j = (i % max + max) % max;

            return list[j];
        }
    }
}
