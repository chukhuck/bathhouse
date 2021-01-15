using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Bathhouse.Entities
{
  public class Office : Entity
  {
    public int Number { get; set; } = 0;
    [MaxLength(150, ErrorMessage ="Max lenght of field is 150 symbols.")]
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(8).AddMinutes(0);
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(22).AddMinutes(0);
    [NotMapped]
    public string WorkingTimeRange => $"{TimeOfOpen.ToShortTimeString()} - {TimeOfClose.ToShortTimeString()}";
    public string? Email { get; set; }

    public ICollection<Employee> Employees { get; set; } = null!;
    public ICollection<Client> Clients { get; set; } = null!;

    public IEnumerable<Employee> GetManagers()
    {
      return Employees.Where(e => e.Type == EmployeeType.Manager);
    }

    public void DeleteEmployee(Guid employeeId)
    {
      if(Employees.FirstOrDefault(e => e.Id == employeeId) is Employee removingEmployee)
        Employees.Remove(removingEmployee);
    }

    public void AddEmployee(Employee addingEmployee)
    {
      Employees.Add(addingEmployee);
    }
  }
}
