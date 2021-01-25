using Bathhouse.Entities;
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
      List<string> footers = new List<string>();

      for (int column = 0; column < Headers.Count; column++)
      {
        Footers.Add(AggregateColumn(index: column, source: Data, datatype: Headers[column].Type)); 
      }

      return footers;
    }

    private string AggregateColumn(int index, List<List<string>> source, SurveySummaryHeaderType datatype)
    {
      List<string> column = ExtractColumn(index: index, source: source);

      return datatype switch
      {
        SurveySummaryHeaderType.Bool => AggregateBoolColumn(column: column),
        SurveySummaryHeaderType.DateTime => AggregateDateTimeColumn(column: column),
        SurveySummaryHeaderType.Decimal => AggregateDecimalColumn(column: column),
        SurveySummaryHeaderType.Number => AggregateNumberColumn(column: column),
        SurveySummaryHeaderType.Text => AggregateTextColumn(column: column),
        SurveySummaryHeaderType.Photo => string.Empty,
        _ => string.Empty
      };
    }

    private string AggregateNumberColumn(List<string> column)
    {
      try
      {
        var temp = column
          .Select(i => int.Parse(i))
          .Sum();

        string preambula = "Total: \r\n";

        string aggregatedValue = string.Concat(preambula, string.Join("\r\n", temp));

        return aggregatedValue;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private string AggregateDecimalColumn(List<string> column)
    {
      try
      {
        var temp = column
          .Select(i => decimal.Parse(i))
          .Sum();

        string preambula = "Total: \r\n";

        string aggregatedValue = string.Concat(preambula, string.Join("\r\n", temp));

        return aggregatedValue;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private string AggregateDateTimeColumn(List<string> column)
    {
      try
      {
        var temp = column.Select(i => DateTime.Parse(i));

        DateTime min = temp.Min();
        DateTime max = temp.Max();

        string preambula = string.Empty;

        string aggregatedValue = string.Concat(preambula, $"From {min} to {max}.");

        return aggregatedValue;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private string AggregateBoolColumn(List<string> column)
    {
      try
      {
        var temp = column
          .Select(i => bool.Parse(i))
          .GroupBy(b => b)
          .Select( g => $"Value: {g.Key}, Count = {g.Count()}");

        string preambula = "Total: \r\n";

        string aggregatedValue = string.Concat(preambula, string.Join("\r\n", temp));

        return aggregatedValue;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private string AggregateTextColumn(List<string> column)
    {
      try
      {
        var temp = column
          .GroupBy(b => b)
          .Select(g => $"Value: {g.Key}, Count = {g.Count()}");

        string preambula = "Total: \r\n";

        string aggregatedValue = string.Concat(preambula, string.Join("\r\n", temp));

        return aggregatedValue;
      }
      catch (Exception)
      {
        throw;
      }
    }

    private List<string> ExtractColumn(int index, List<List<string>> source)
    {
      return source.Select(row => row[index]).ToList();
    }
  }
}