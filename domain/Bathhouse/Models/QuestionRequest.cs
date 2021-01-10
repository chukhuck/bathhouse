using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class QuestionRequest
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
  }
}
