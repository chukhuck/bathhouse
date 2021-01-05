using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Models
{
  public class AnswerModel : EntityModel
  {
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [DataType(DataType.Text)]
    public string Value { get; set; }

    [DataType(DataType.Text)]
    public string EmployeeName { get; set; }

    [DataType(DataType.Text)]
    public string EmployeeOffice { get; set; }
  }
}