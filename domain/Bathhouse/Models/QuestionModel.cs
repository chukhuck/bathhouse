using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Models
{
  public class QuestionModel : EntityModel
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Текст нового вопроса")]
    public string Text { get; set; } = "Текст нового вопроса";

    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    [DefaultValue("Вопрос 1")]
    public string Name { get; set; } = "Вопрос 1";

    [DefaultValue(false)]
    public bool IsKey { get; set; } = false;

//    public ICollection<AnswerModel> Answers { get; set; }
  }
}