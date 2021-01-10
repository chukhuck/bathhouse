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

    [MaxLength(150, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 150 symbols.")]
    public string Value { get; set; }
  }
}