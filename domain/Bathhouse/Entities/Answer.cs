using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Answer : Entity
  {
    public Question Question { get; set; }

    public Guid QuestionId { get; set; }

    public SurveyResult Result { get; set; }

    public Guid ResultId { get; set; }

    [DataType(DataType.Text)]
    public string Value { get; set; }
  }
}