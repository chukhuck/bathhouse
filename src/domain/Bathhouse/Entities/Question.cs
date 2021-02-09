using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;

namespace Bathhouse.Entities
{
  public class Question : IEntity<Guid>
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = "New question";
    public string Name { get; set; } = "Newquestion";
    public bool IsKey { get; set; } = false;
    public QuestionType Type { get; set; } = QuestionType.Number;

    public virtual Survey Survey { get; set; } = null!;
    public Guid SurveyId { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = null!;
  }
}