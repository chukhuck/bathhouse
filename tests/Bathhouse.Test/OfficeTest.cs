using Bathhouse.Entities;
using Bathhouse.Memory;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class OfficeTest
  {
    public static readonly Office office = InMemoryContext.Offices.FirstOrDefault();

    [Fact]
    public void GetManagers()
    {
      List<Employee> managers = office.Employees.ToList();//.Where(e => e.Type == EmployeeType.Manager).ToList();

      Assert.Equal(managers, office.GetManagers());
    }

    [Fact]
    public void Add_Employee_Equal_Null()
    {
      Assert.Throws<ArgumentNullException>(() => office.AddEmployee(null));
    }

    [Fact]
    public void Add_Employee_Not_Equal_Null()
    {
      Employee employee = new Employee();

      office.AddEmployee(employee);

      Assert.Single(office.Employees, o => o.Id == employee.Id);
    }

    [Fact]
    public void Delete_Exist_Employee()
    {
      Guid idDeletingEmployee = InMemoryContext.Offices.Where(e => e.Employees.Count != 0).LastOrDefault().Id;

      office.DeleteEmployee(idDeletingEmployee);

      Assert.DoesNotContain(idDeletingEmployee, office.Employees.Select(o => o.Id));
    }

    [Fact]
    public void Delete_Not_Exist_Employee()
    {
      Office off = InMemoryContext.Offices.Where(e => e.Employees.Count != 0).LastOrDefault();

      int employeeCount = off.Employees.Count;

      off.DeleteEmployee(Guid.NewGuid());

      Assert.Equal(employeeCount, off.Employees.Count);
    }
  }
}
