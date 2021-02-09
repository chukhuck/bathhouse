using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Contracts.Models
{
  public class SurveyRequest
  {
    [Required(ErrorMessage = "Field Name is required.")]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    [DefaultValue("Новый опрос")]
    public string Name { get; set; } = "Новый опрос";

    [DataType(DataType.Text)]
    [StringLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Описание нового опроса")]
    public string? Description { get; set; } = "Описание нового опроса";

    [Required]
    public Guid AuthorId { get; set; }

    public ICollection<QuestionRequest> Questions { get; set; } = new List<QuestionRequest>();
  }
}
