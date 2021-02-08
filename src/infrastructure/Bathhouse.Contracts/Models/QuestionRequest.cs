using Bathhouse.ValueTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Contracts.Models
{
  public class QuestionRequest
  {
    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Текст нового вопроса")]
    public string Text { get; set; } = "Текст нового вопроса";

    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    [DefaultValue("Вопрос 1")]
    public string Name { get; set; } = "Вопрос 1";

    [DefaultValue(false)]
    public bool IsKey { get; set; } = false;

    [DefaultValue(QuestionType.Number)]
    public QuestionType Type { get; set; } = QuestionType.Number;
  }
}
