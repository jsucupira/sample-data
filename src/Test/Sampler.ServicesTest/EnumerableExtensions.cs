using System;
using System.Collections.Generic;

namespace Sampler.Test
{
    /// <summary>
    /// Got this from 
    /// http://www.geekality.net/2010/01/19/how-to-check-for-duplicates/
    /// </summary>
    public static class EnumerableExtensions
    {
        public static bool HasDuplicates<T>(this IEnumerable<T> subjects)
        {
            return HasDuplicates(subjects, EqualityComparer<T>.Default);
        }

        public static bool HasDuplicates<T>(this IEnumerable<T> subjects, IEqualityComparer<T> comparer)
        {
            if (subjects == null)
                throw new ArgumentNullException("subjects");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            var set = new HashSet<T>(comparer);

            foreach (var s in subjects)
            {
                if (!set.Add(s))
                    return true;
            }

            return false;
        }
    }
}