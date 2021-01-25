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
    public List<string> Footers { get; private set; } = new List<string>();


    public static BaseSurveySummary Create(Survey survey) 
    {
      BaseSurveySummary summary = new BaseSurveySummary();

      summary.Survey = survey;

      summary.Headers = summary.GetHeaders();
      summary.Data = summary.GetData();
      summary.Footers = summary.GetFooters();

      return summary;
    }

    /// <summary>
    /// Get footers for this survey
    /// </summary>
    /// <returns>List of footers by column</returns>
    protected virtual List<string> GetFooters() => new ();

    /// <summary>
    /// Get all of results of this survey as List
    /// </summary>
    /// <returns></returns>
    protected virtual List<List<string>> GetData() => Survey?.Results?.Select(r => r.ToList()).ToList() ?? new List<List<string>>();

    /// <summary>
    /// Get headrers for this survey
    /// </summary>
    /// <returns>List of headers</returns>
    protected virtual List<SurveySummaryHeader> GetHeaders()
    {
      List<SurveySummaryHeader> headers = new();

      headers.Add(new SurveySummaryHeader() { Type = SurveySummaryHeaderType.DateTime, Text = "Date" });
      headers.Add(new SurveySummaryHeader() { Type = SurveySummaryHeaderType.Text, Text = "Author" });

      headers.AddRange(
        Survey?.Questions?.Select(q => new SurveySummaryHeader() { Type = SurveySummaryHeader.ConvertType(q.Type), Text = q.Name }) 
        ?? new List<SurveySummaryHeader>());

      return headers;
    }
  }

  public struct SurveySummaryHeader
  {
    public SurveySummaryHeaderType Type { get; set; }
    public string Text { get; set; }

    public static SurveySummaryHeaderType ConvertType(QuestionType questionType)
    {
      return questionType switch
      {
        QuestionType.DateTime => SurveySummaryHeaderType.DateTime,
        QuestionType.Decimal => SurveySummaryHeaderType.Decimal,
        QuestionType.Number => SurveySummaryHeaderType.Number,
        QuestionType.YesNo => SurveySummaryHeaderType.Bool,
        QuestionType.Photo => SurveySummaryHeaderType.Text,
        QuestionType.Text => SurveySummaryHeaderType.Text,
        _ => SurveySummaryHeaderType.Text
      };
    }
  }

  public enum SurveySummaryHeaderType
  {
    Text,
    Number,
    Decimal,
    DateTime,
    Bool,
    Photo
  }
}