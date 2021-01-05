using System;
using System.Collections.Generic;

namespace Bathhouse.Models
{
  public class SurveyResultModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<SurveyResultHeader> Headers { get; set; }
    public List<List<string>> Data { get; set; }
  }


  public struct SurveyResultHeader
  {
    public SurveyResultHeaderType Type { get; set; }
    public string Text { get; set; }
  }

  public enum SurveyResultHeaderType
  {
    Text,
    Number,
    Decimal,
    Datetime,
    Bool
  }
}