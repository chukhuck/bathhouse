using Bathhouse.Entities;
using System;

namespace Bathhouse.ValueTypes
{
  internal class SurveySummaryFactory
  {
    internal static BaseSurveySummary Create(Survey survey, SurveyResultSummaryType typeSummary)
    {
      return typeSummary switch
      {
        SurveyResultSummaryType.Base => BaseSurveySummary.Create(survey),
        SurveyResultSummaryType.Aggregated => AggregatedSurveySummary.Create(survey),
        _ => throw new ArgumentException("Type of summary is not defined.")
      };
    }
  }
}