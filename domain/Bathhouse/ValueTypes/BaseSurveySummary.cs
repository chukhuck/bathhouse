using Bathhouse.Entities;
using Bathhouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.ValueTypes
{
  public class BaseSurveySummary
  {
    private BaseSurveySummary(){}

    public Survey Survey { get; private set; } = null!;
    public List<SurveySummaryHeader> Headers { get; private set; } = new List<SurveySummaryHeader>();
    public List<List<string>> Data { get; private set; } = new List<List<string>>();


    public static BaseSurveySummary Create(Survey survey) 
    {
      BaseSurveySummary summary = new BaseSurveySummary();

      summary.Survey = survey;

      summary.Headers = summary.GetHeaders();
      summary.Data = summary.GetData();

      return summary;
    }

    /// <summary>
    /// Get all of results of this survey as List
    /// </summary>
    /// <returns></returns>
    public virtual List<List<string>> GetData() => Survey?.Results?.Select(r => r.ToList()).ToList() ?? new List<List<string>>();

    /// <summary>
    /// Get headrers for this survey
    /// </summary>
    /// <returns>List of headers</returns>
    public virtual List<SurveySummaryHeader> GetHeaders()
    {
      List<SurveySummaryHeader> headers = new();

      headers.Add(new SurveySummaryHeader() { Type = SurveySummaryHeaderType.Datetime, Text = "Date" });
      headers.Add(new SurveySummaryHeader() { Type = SurveySummaryHeaderType.Text, Text = "Employee" });

      headers.AddRange(
        Survey?.Questions?.Select(q => new SurveySummaryHeader() { Type = SurveySummaryHeaderType.Text, Text = q.Name }) 
        ?? new List<SurveySummaryHeader>());

      return headers;
    }
  }

  public struct SurveySummaryHeader
  {
    public SurveySummaryHeaderType Type { get; set; }
    public string Text { get; set; }
  }

  public enum SurveySummaryHeaderType
  {
    Text,
    Number,
    Decimal,
    Datetime,
    Bool
  }
}