using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using Chuk.Helpers.Linq;
using System;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class OfficeTest : IClassFixture<BathhouseDbFixture>
  {
    public OfficeTest(BathhouseDbFixture fixture) => Fixture = fixture;

    public BathhouseDbFixture Fixture { get; }


    [Fact]
    public void Create_Office_With_Default_Values()
    {
      Office office = new();

      Assert.Equal(0, office.Number);
      Assert.Equal(DateTime.Parse("0001-01-01T08:00:00"), office.TimeOfOpen);
      Assert.Equal(DateTime.Parse("0001-01-01T22:00:00"), office.TimeOfClose);
      Assert.Equal("8:00 - 22:00", office.WorkingTimeRange);
    }


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
