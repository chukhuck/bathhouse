using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;

namespace Bathhouse.Contracts.v1.Models
{
  public class SurveySummaryResponse
  {
    public Guid SurveyId { get; set; }
    public string? SurveyName { get; set; }
    public List<SurveySummaryHeader> Headers { get; set; } = null!;
    public List<List<string>> Data { get; set; } = null!;
    public List<string> Footers { get; set; } = null!;
  }
}