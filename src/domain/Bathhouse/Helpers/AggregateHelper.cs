using Bathhouse.ValueTypes;
using Chuk.Helpers.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Helpers
{
  public static class AggregateHelper
  {
    public static string AggregateColumn(this IEnumerable<IEnumerable<string>> source, int index, DataType datatype)
    {
      IEnumerable<string> column = source.ExtractColumn(index);

      return datatype switch
      {
        DataType.Bool => column.Select(c => c == "true" ? "YES" : "NO").GroupedAndCount(separator: "\r\n", valueLabel: "Value: ", countLabel: ", Count = "),
        DataType.DateTime => column.Select(c => DateTime.Parse(c)).MinMax(prefix: "From ", separator: " to "),
        DataType.Decimal => column.Select(c => decimal.Parse(c)).SumAsDecimal(prefix: "Total: "),
        DataType.Number => column.Select(c => int.Parse(c)).SumAsDecimal(prefix: "Total: "),
        DataType.Text => column.GroupedAndCount(separator: "\r\n", valueLabel: "Value: ", countLabel: ", Count = "),
        DataType.Photo => string.Empty,
        _ => string.Empty
      };
    }
  }
}
