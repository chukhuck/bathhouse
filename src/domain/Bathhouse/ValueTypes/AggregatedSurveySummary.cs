using Bathhouse.Entities;
using Bathhouse.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.ValueTypes
{
  public class AggregatedSurveySummary : SimpleSurveySummary
  {
    protected internal AggregatedSurveySummary(Survey survey) : base(survey) { }


    protected override sealed List<string> GetFooters()
    {
      return Headers.Select((header, index) => AggregateColumn(index, Data, header.Type)).ToList();
    }

    private string AggregateColumn(int index, List<List<string>> source, SurveySummaryHeaderType datatype)
    {
      List<string> column = ExtractColumn(index: index, source: source);

      return datatype switch
      {
        SurveySummaryHeaderType.Bool => DataAggregator.GroupedAndCount(column.Select(c => c == "true" ? "YES": "NO"), separator: "\r\n", valueLabel: "Value: ", countLabel: ", Count = "),
        SurveySummaryHeaderType.DateTime => DataAggregator.MinMax(column.Select(c => DateTime.Parse(c)), preambola: "From ", separator: " to "),
        SurveySummaryHeaderType.Decimal => DataAggregator.Sum(column.Select(c => decimal.Parse(c)), preambola: "Total: "),
        SurveySummaryHeaderType.Number => DataAggregator.Sum(column.Select(c => int.Parse(c)), preambola: "Total: "),
        SurveySummaryHeaderType.Text => DataAggregator.GroupedAndCount(column, separator: "\r\n", valueLabel: "Value: ", countLabel: ", Count = "),
        SurveySummaryHeaderType.Photo => string.Empty,
        _ => string.Empty
      };
    }

    private List<string> ExtractColumn(int index, List<List<string>> source)
    {
      return source.Select(row => row[index]).ToList();
    }
  }
}