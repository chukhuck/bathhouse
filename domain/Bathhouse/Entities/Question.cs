using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Question : Entity
  {
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Text { get; set; } = "New question";
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    public string Name { get; set; } = "Newquestion";
    public bool IsKey { get; set; } = false;
    public QuestionType Type { get; set; } = QuestionType.Number;

    public Survey Survey { get; set; } = null!;
    public Guid SurveyId { get; set; }

    public ICollection<Answer> Answers { get; set; } = null!;
  }

  public enum QuestionType
  {
    Number,
    Decimal,
    YesNo,
    Text,
    Photo,
    DateTime
  }
}