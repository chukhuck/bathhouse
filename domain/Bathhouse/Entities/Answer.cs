using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Answer : Entity
  {
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public Question Question { get; set; }

    public Guid QuestionId { get; set; }

    [DataType(DataType.Text)]
    public string Value { get; set; }

    public Employee Author { get; set; }

    public Guid AuthorId { get; set; }
  }
}