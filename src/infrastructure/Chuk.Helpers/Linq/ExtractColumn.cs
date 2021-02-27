using System;
using System.Collections.Generic;
using System.Linq;

namespace Chuk.Helpers.Linq
{
  public static partial class EnumerableExtension
  {
    public static IEnumerable<TSource> ExtractColumn<TSource>(this IEnumerable<IEnumerable<TSource>> source, int index)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      return source.Select(row => row.ElementAt(index));
    }
  }
}
