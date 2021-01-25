using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Helpers
{
  public static class DataAggregator
  {
    public static decimal Sum<T>(IEnumerable<T> data)
    {
      try
      {
        if (data == null)
          throw new ArgumentNullException(nameof(data));

        return data.Select(d=> decimal.Parse(d?.ToString() ?? throw new ArgumentNullException("Element of" + nameof(data)))).Sum();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string Sum<T>(IEnumerable<T> data, string preambola)
    {
      return string.Concat(preambola, Sum(data));
    }

    public static string Append(IEnumerable<string> data, string preambula = "", string separator = "\r\n")
    {
      try
      {
        if (data == null)
          throw new ArgumentNullException(nameof(data));

        return preambula + string.Join(separator, data);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static (T? min, T? max) MinMax<T>(IEnumerable<T> data)
    {
      try
      {
        if (data == null)
          throw new ArgumentNullException(nameof(data));

        return (min: data.Min(), max: data.Max());
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string MinMax<T>(IEnumerable<T> data, string preambola, string separator)
    {
      try
      {
        var minmax = MinMax(data);
        return string.Concat(preambola, minmax.min, separator, minmax.max);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static IEnumerable<(T value, int count)> GroupedAndCount<T>(IEnumerable<T> data)
    {
      try
      {
        if (data == null)
          throw new ArgumentNullException(nameof(data));

        return data.GroupBy(d => d).Select(g => (g.Key, g.Count()));
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string GroupedAndCount<T>(IEnumerable<T> data, string separator, string valueLabel, string countLabel)
    {
      try
      {
        return string.Join(separator, GroupedAndCount(data).Select(d => valueLabel + d.value + countLabel + d.count));
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
