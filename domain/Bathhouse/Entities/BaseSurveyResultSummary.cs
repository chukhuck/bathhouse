using Bathhouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Entities
{
  public class BaseSurveyResultSummary
  {
    private BaseSurveyResultSummary(){}

    public Survey Survey { get; private set; }
    public List<SurveySummaryHeader> Headers { get; private set; }
    public List<List<string>> Data { get; private set; }


    public static BaseSurveyResultSummary Create(Survey survey) 
    {
      BaseSurveyResultSummary summary = new BaseSurveyResultSummary();

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