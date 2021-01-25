using Bathhouse.ValueTypes;
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

    public static string AggregateColumn(this IEnumerable<IEnumerable<string>> source, int index, DataType datatype)
    {
      IEnumerable<string> column = source.ExtractColumn(index);

      return datatype switch
      {
        DataType.Bool => column.Select(c => c == "true" ? "YES" : "NO").GroupedAndCount(separator: "\r\n", valueLabel: "Value: ", countLabel: ", Count = "),
        DataType.DateTime => column.Select(c => DateTime.Parse(c)).MinMax(preambola: "From ", separator: " to "),
        DataType.Decimal => column.Select(c => decimal.Parse(c)).Sum(preambola: "Total: "),
        DataType.Number => column.Select(c => int.Parse(c)).Sum(preambola: "Total: "),
        DataType.Text => column.GroupedAndCount(separator: "\r\n", valueLabel: "Value: ", countLabel: ", Count = "),
        DataType.Photo => string.Empty,
        _ => string.Empty
      };
    }
  }
}
