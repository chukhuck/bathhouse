using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using chukhuck.Linq.Extensions;
using System;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class OfficeTest : IClassFixture<SharedBathhouseDbFixture>
  {
    public OfficeTest(SharedBathhouseDbFixture fixture) => Fixture = fixture;

    public SharedBathhouseDbFixture Fixture { get; }

    [Fact]
    public void Add_Employee_Equal_Null()
    {
      using var context = Fixture.CreateContext();
      var office = context.Offices.ToList().RandomOrDefault() ?? new Office();

#pragma warning disable CS8625
      Assert.Throws<ArgumentNullException>(() => office.AddEmployee(null));
#pragma warning restore CS8625
    }

    [Fact]
    public void Add_Employee_Not_Equal_Null()
    {
      using var context = Fixture.CreateContext();
      var office = context.Offices.ToList().RandomOrDefault() ?? new Office();

      Employee employee = new Employee();

      office.AddEmployee(employee);

      Assert.Single(office.Employees, o => o.Id == employee.Id);
    }

    [Fact]
    public void Delete_Exist_Employee()
    {
      using var context = Fixture.CreateContext();
      var office = context.Offices.ToList().RandomOrDefault() ?? new Office();

      Guid idDeletingEmployee = office.Employees.Last().Id;

      office.DeleteEmployee(idDeletingEmployee);

      Assert.DoesNotContain(idDeletingEmployee, office.Employees.Select(o => o.Id));
    }

    [Fact]
    public void Delete_Not_Exist_Employee()
    {
      using var context = Fixture.CreateContext();
      var office = context.Offices.ToList().RandomOrDefault() ?? new Office();

      int employeeCount = office.Employees.Count;

      office.DeleteEmployee(Guid.NewGuid());

      Assert.Equal(employeeCount, office.Employees.Count);
    }
  }
}
