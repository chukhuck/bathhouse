using System;
using System.Collections.Generic;
using System.Linq;

namespace chukhuck.Helpers.Linq.Extensions
{
  public static class DataAggregator
  {
    public static decimal Sum<T>(this IEnumerable<T> data)
    {
      try
      {
        if (data == null)
          throw new ArgumentNullException(nameof(data));

        return data.Select(d => decimal.Parse(d?.ToString() ?? throw new ArgumentNullException(nameof(data)))).Sum();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static string Sum<T>(this IEnumerable<T> data, string preambola)
    {
      return string.Concat(preambola, Sum(data));
    }

    public static string Append(this IEnumerable<string> data, string preambula = "", string separator = "\r\n")
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

    public static (T? min, T? max) MinMax<T>(this IEnumerable<T> data)
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

    public static string MinMax<T>(this IEnumerable<T> data, string preambola, string separator)
    {
      try
      {
        var (min, max) = data.MinMax();
        return string.Concat(preambola, min, separator, max);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static IEnumerable<(T value, int count)> GroupedAndCount<T>(this IEnumerable<T> data)
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

    public static string GroupedAndCount<T>(this IEnumerable<T> data, string separator, string valueLabel, string countLabel)
    {
      try
      {
        return string.Join(separator, data.GroupedAndCount().Select(d => valueLabel + d.value + countLabel + d.count));
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static IEnumerable<T> ExtractColumn<T>(this IEnumerable<IEnumerable<T>> source, int index)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      return source.Select(row => row.ElementAt(index)).ToList();
    }
  }
}
