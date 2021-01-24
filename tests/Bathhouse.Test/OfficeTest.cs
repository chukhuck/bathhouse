using Bathhouse.Entities;
using Bathhouse.Memory;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Bathhouse.Test
{
  public class OfficeTest
  {
    public static readonly Office office = InMemoryContext.Offices.FirstOrDefault();

    [Fact]
    public void GetManagers()
    {
      List<Employee> managers = office.Employees.Where(e => e.Type == EmployeeType.Manager).ToList();

      Assert.Equal(managers, office.GetManagers());
    }
  }
}
