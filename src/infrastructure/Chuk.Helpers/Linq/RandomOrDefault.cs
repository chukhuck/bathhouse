using System;
using System.Collections.Generic;
using System.Linq;

namespace Chuk.Helpers.Linq
{
  public static partial class EnumerableExtension
  {
    public static TSource? RandomOrDefault<TSource>(this IEnumerable<TSource> source)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      int count = source.Count();

      if (count == 0)
        return default;

      Random randomiser = new Random();
      int index = randomiser.Next(minValue: 0, maxValue: count);
      return source.ElementAt(index);
    }
  }
}
