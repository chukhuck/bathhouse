using Bathhouse.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.ValueTypes
{
  public class SimpleSurveySummary : SurveySummary
  {
    protected internal SimpleSurveySummary(Survey survey) : base(survey) { }

    /// <summary>
    /// Get footers for this survey
    /// </summary>
    /// <returns>List of footers by column</returns>
    protected override List<string> GetFooters() => new() { $"Total: {Data.Count}"};

    /// <summary>
    /// Get all of results of this survey as List
    /// </summary>
    /// <returns></returns>
    protected override List<List<string>> GetData()
    {
      if (Survey is null || Survey.Results is null || Survey.Questions is null)
      {
        return new List<List<string>>();
      }

      return Survey.GetResultsForQuestions(Survey.Questions);
       
    }

    /// <summary>
    /// Get headrers for this survey
    /// </summary>
    /// <returns>List of headers</returns>
    protected override List<SurveySummaryHeader> GetHeaders()
    {
      List<SurveySummaryHeader> headers = new();

      headers.Add(new SurveySummaryHeader() { Type = DataType.DateTime, Text = "Date" });
      headers.Add(new SurveySummaryHeader() { Type = DataType.Text, Text = "Author" });

      headers.AddRange(
        Survey?.Questions?
        .Select(q => new SurveySummaryHeader() 
            { 
                Type = SurveySummaryHeader.ConvertType(q.Type), 
                Text = q.Name 
            })
        ?? new List<SurveySummaryHeader>());

      return headers;
    }
  }
}
