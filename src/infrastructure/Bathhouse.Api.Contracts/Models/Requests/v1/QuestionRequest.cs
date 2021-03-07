using Bathhouse.ValueTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Api.Contracts.Models.Requests.v1
{
  public class QuestionRequest
  {
    [Required(ErrorMessage = "Field Text is required.")]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [StringLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Текст нового вопроса")]
    public string Text { get; set; } = "Текст нового вопроса";

    [Required(ErrorMessage = "Field Name is required.")]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [StringLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    [DefaultValue("Вопрос 1")]
    public string Name { get; set; } = "Вопрос 1";

    [DefaultValue(false)]
    public bool IsKey { get; set; } = false;

    [DefaultValue(QuestionType.Number)]
    [EnumDataType(typeof(QuestionType), ErrorMessage = "Incorrect a data type.")]
    public QuestionType Type { get; set; } = QuestionType.Number;
  }
}
