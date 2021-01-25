using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.ValueTypes
{
  public class SimpleSurveySummary : SurveySummary
  {
    protected internal SimpleSurveySummary(Survey survey) : base(survey) { }

    /// <summary>
    /// Get footers for this survey
    /// </summary>
    /// <returns>List of footers by column</returns>
    protected override List<string> GetFooters() => new();

    /// <summary>
    /// Get all of results of this survey as List
    /// </summary>
    /// <returns></returns>
    protected override List<List<string>> GetData() => Survey?.Results?.Select(r => r.ToList()).ToList() ?? new List<List<string>>();

    /// <summary>
    /// Get headrers for this survey
    /// </summary>
    /// <returns>List of headers</returns>
    protected override List<SurveySummaryHeader> GetHeaders()
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
}
