using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
  public class Survey : Entity
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    public string Name { get; set; } = "Новый опрос";

    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Description { get; set; } = "Описание нового опроса";

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now; 

    public ICollection<Question> Questions { get; set; }
  }
}
