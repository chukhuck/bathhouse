using Bathhouse.Entities;
using System;
using System.Collections.Generic;

namespace Bathhouse.ValueTypes
{
  public abstract class SurveySummary
  {
    private SurveySummary() { }

    protected internal SurveySummary(Survey survey)
    {
      Survey = survey ?? throw new ArgumentNullException(paramName: nameof(survey));
    }

    public Survey Survey { get; private set; } = null!;
    public List<SurveySummaryHeader> Headers { get; private set; } = new List<SurveySummaryHeader>();
    public List<List<string>> Data { get; private set; } = new List<List<string>>();
    public List<string> Footers { get; private set; } = new List<string>();

    public static SurveySummary Create(Survey survey, SurveyResultSummaryType typeSummary)
    {
      SurveySummary summary = SurveySummaryFactory.Create(survey, typeSummary);

      summary.Headers = summary.GetHeaders();
      summary.Data = summary.GetData();
      summary.Footers = summary.GetFooters();

      return summary;
    }

    /// <summary>
    /// Get footers for this survey
    /// </summary>
    /// <returns>List of footers by column</returns>
    protected abstract List<string> GetFooters();

    /// <summary>
    /// Get all of results of this survey as List
    /// </summary>
    /// <returns></returns>
    protected abstract List<List<string>> GetData();

    /// <summary>
    /// Get headrers for this survey
    /// </summary>
    /// <returns>List of headers</returns>
    protected abstract List<SurveySummaryHeader> GetHeaders();
  }
}