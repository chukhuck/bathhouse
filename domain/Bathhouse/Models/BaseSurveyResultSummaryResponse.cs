using Bathhouse.Entities;
using System;
using System.Collections.Generic;

namespace Bathhouse.Models
{
  public class BaseSurveyResultSummaryResponse
  {
    public Guid SurveyId { get; set; }
    public string SurveyName { get; set; }
    public List<SurveySummaryHeader> Headers { get; set; }
    public List<List<string>> Data { get; set; }
  }
}