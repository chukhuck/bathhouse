using System;
using System.Collections.Generic;

namespace Bathhouse.Contracts.Models.v1.Responses
{
  public class SurveySummaryResponse
  {
    public Guid SurveyId { get; set; }
    public string? SurveyName { get; set; }
    public List<SurveySummaryHeaderResponse> Headers { get; set; } = null!;
    public List<List<string>> Data { get; set; } = null!;
    public List<string> Footers { get; set; } = null!;
  }
}