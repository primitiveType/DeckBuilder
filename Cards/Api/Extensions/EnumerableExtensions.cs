using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable, Random random, Func<T, bool> constraint = null)
        {
            if (constraint == null)
            {
                int index = random.SystemRandom.Next(0, enumerable.Count());
                return enumerable.ElementAt(index);
            }
            else
            {
                IEnumerable<T> constrained = enumerable.Where(constraint);
                int index = random.SystemRandom.Next(0, constrained.Count());
                return constrained.ElementAt(index);
            }
        }
    }
}
