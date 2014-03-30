using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Utilities
{
    /// <summary>
    /// Extension methods for cyclically indexing into a IList.
    /// </summary>
    public static class CyclicIndexingExtensions
    {
        /// <summary>
        /// Returns the object at the i mod (list.Count)th index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static T AtCyclicIndex<T>(this IList<T> list, int i)
        {
            int max = list.Count;
            
            // Calculate (i mod (list.Count)). This is different from i%list.Count, which would be the remainder.
            int j = (i % max + max) % max;

            return list[j];
        }
    }
}
