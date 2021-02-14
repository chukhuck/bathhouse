using Chuk.Helpers.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Entities
{
  public class SurveyResult : IEntity<Guid>
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public virtual Employee? Author { get; set; }
    public Guid? AuthorId { get; set; }
    public virtual Survey Survey { get; set; } = null!;
    public Guid SurveyId { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = null!;


    public virtual List<string> ToList()
    {
      List<string> row = new ();

      row.Add(CreationDate.ToString());
      row.Add(Author?.LastName ?? "Anonim");

      row.AddRange(Answers.Select(a => a.Value));

      return row;
    }
  }
}