using System.Collections.Generic;
using System.Linq;

namespace Chuk.Helpers.Linq
{
  public static partial class EnumerableExtension
  {
    public static string SumAsDecimal<TSource>(this IEnumerable<TSource> source, string prefix)
    {
      return string.Concat(prefix, source.Sum(s => decimal.Parse(s?.ToString() ?? "0")));
    }
  }
}
