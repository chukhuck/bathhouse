using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Answer : Entity
  {
    public Question Question { get; set; } = null!;

    public Guid QuestionId { get; set; }

    public SurveyResult Result { get; set; } = null!;

    public Guid ResultId { get; set; }

    [MaxLength(150, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 150 symbols.")]
    public string Value { get; set; } = string.Empty;
  }
}