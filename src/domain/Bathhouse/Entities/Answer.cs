using System;

namespace Bathhouse.Entities
{
  public class Answer
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Question Question { get; set; } = null!;

    public Guid QuestionId { get; set; }

    public SurveyResult Result { get; set; } = null!;

    public Guid ResultId { get; set; }

    public string Value { get; set; } = string.Empty;
  }
}