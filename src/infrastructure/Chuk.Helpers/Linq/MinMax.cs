using System;
using System.Collections.Generic;
using System.Linq;

namespace Chuk.Helpers.Linq
{
  public static partial class EnumerableExtension
  {
    public static (TSource? min, TSource? max) MinMax<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      return (min: source.Min(), max: source.Max());
    }

    public static string MinMax<TSource>(this IEnumerable<TSource> data, string prefix, string separator)
    {
      string realPrefix = prefix ?? string.Empty;
      string realSeparator = separator ?? string.Empty;

      var (min, max) = data.MinMax();

      return string.Concat(realPrefix, min, realSeparator, max);

    }
  }
}
