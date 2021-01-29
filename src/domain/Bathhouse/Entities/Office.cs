﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Entities
{
  public class Office
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Number { get; set; } = 0;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(8).AddMinutes(0);
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(22).AddMinutes(0);
    public string WorkingTimeRange => $"{TimeOfOpen.ToShortTimeString()} - {TimeOfClose.ToShortTimeString()}";
    public string? Email { get; set; }


    public ICollection<Employee> Employees { get; set; } = null!;
    public ICollection<Client> Clients { get; set; } = null!;

    public IEnumerable<Employee> GetManagers()
    {
      return Employees;//.Where(e => e.Type == EmployeeType.Manager);
    }

    public void DeleteEmployee(Guid employeeId)
    {
      if(Employees.FirstOrDefault(e => e.Id == employeeId) is Employee removingEmployee)
        Employees.Remove(removingEmployee);
    }

    public void AddEmployee(Employee addingEmployee)
    {
      if (addingEmployee == null)
        throw new ArgumentNullException(paramName: nameof(addingEmployee));

      Employees.Add(addingEmployee);
    }
  }
}
