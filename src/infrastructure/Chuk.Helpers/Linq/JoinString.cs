using System;
using System.Collections.Generic;

namespace Chuk.Helpers.Linq
{
  public static partial class EnumerableExtension
  {
    public static string JoinString<TSource>(
      this IEnumerable<TSource> source, 
      string preambula = "", 
      string separator = "\r\n")
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      return preambula + string.Join(separator, source);
    }
  }
}
