using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class SurveyResultModel
  {
    [DataType(DataType.Text)]
    public string Name { get; set; }

    [DataType(DataType.Text)]
    public string Description { get; set; } 

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; }

    public ICollection<AnswerModel> Answers { get; set; }
  }
}
