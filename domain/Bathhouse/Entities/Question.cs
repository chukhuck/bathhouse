using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Question : Entity
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Text { get; set; } = "";

    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    public string Name { get; set; } = "";

    public bool IsKey { get; set; } = false;

    public Survey Survey { get; set; }

    public Guid SurveyId { get; set; }

    public ICollection<Answer> Answers { get; set; }
  }
}