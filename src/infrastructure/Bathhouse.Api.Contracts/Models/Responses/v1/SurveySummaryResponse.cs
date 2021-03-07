using System;
using System.Collections.Generic;

namespace Bathhouse.Api.Contracts.Models.Responses.v1
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