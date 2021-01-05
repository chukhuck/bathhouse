using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Office : Entity
  {
    [Required(AllowEmptyStrings =false, ErrorMessage ="This field is required for filling.")]
    public int Number { get; set; } = 0;
    [MaxLength(150, ErrorMessage ="Max lenght of field is 150 symbols.")]
    public string Address { get; set; } = "Москва, ";
    [Phone(ErrorMessage="Incorrect phone format.")]
    public string Phone { get; set; } = "+7-495-000-00-00";
    [DataType(DataType.Time, ErrorMessage ="Incorrect time format.")]
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(8).AddMinutes(0);
    [DataType(DataType.Time, ErrorMessage = "Incorrect time format.")]
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(22).AddMinutes(0);


    public ICollection<Employee> Employees { get; set; }
  }
}
