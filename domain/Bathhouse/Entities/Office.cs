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
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(8).AddMinutes(0);
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(22).AddMinutes(0);
    public string WorkingTimeRange => $"{TimeOfOpen.ToShortTimeString()} - {TimeOfClose.ToShortTimeString()}";
    public string Email { get; set; }

    public Employee Manager { get; set; }
    public Guid? ManagerId { get; set; }

    public ICollection<Employee> Employees { get; set; }
    public ICollection<Client> Clients { get; set; }
  }
}
