using System;
using System.Collections.Generic;
using System.Linq;

namespace Chuk.Helpers.Linq
{
  public static partial class EnumerableExtension
  {
    public static IEnumerable<(TSource value, int count)> GroupedAndCount<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));
      
      return source.GroupBy(d => d).Select(g => (g.Key, g.Count()));
    }

    public static string GroupedAndCount<TSource>(this IEnumerable<TSource> source, string separator, string valueLabel, string countLabel)
    {
      return string
        .Join(
          separator, 
          source.GroupedAndCount().Select(d => valueLabel + d.value + countLabel + d.count));
    }
  }
}
