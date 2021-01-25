﻿using Bathhouse.Entities;
using System;

namespace Bathhouse.ValueTypes
{
  internal class SurveySummaryFactory
  {
    public static SurveySummary Create(Survey survey, SurveyResultSummaryType typeSummary)
    {
      return typeSummary switch
      {
        SurveyResultSummaryType.Simple => new SimpleSurveySummary(survey),
        SurveyResultSummaryType.Aggregated => new AggregatedSurveySummary(survey),
        _ => throw new ArgumentException("Type of summary is not defined.")
      };
    }
  }
}