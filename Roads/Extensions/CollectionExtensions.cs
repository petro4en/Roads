using System;
using System.Collections.Generic;

namespace Roads.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsSorted<T>(this IList<T> list, bool ascending = true) where T : IComparable
        {
            if (list.Count < 2)
            {
                return true;
            }

            Func<T, T, bool> isNotMatch = ascending ?
                (T item1, T item2) => item1.CompareTo(item2) == 1 :
                (T item1, T item2) => item1.CompareTo(item2) == -1;

            for (int i = 1; i < list.Count; i++)
            {
                if (isNotMatch(list[i - 1], list[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
