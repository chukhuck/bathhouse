using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chukhuck.Helpers.Linq.Extensions
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
