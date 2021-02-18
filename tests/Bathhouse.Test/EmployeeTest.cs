using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using Chuk.Helpers.Linq.Extensions;
using System;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class EmployeeTest : IClassFixture<BathhouseDbFixture>
  {
    public EmployeeTest(BathhouseDbFixture fixture) => Fixture = fixture;

    public BathhouseDbFixture Fixture { get; }

    [Fact]
    public void Create_Employee_With_Default_Values()
    {
      Employee employee = new();

      Assert.Equal("DefaultLastName", employee.LastName);
      Assert.Equal(string.Empty, employee.FirstName);
      Assert.Equal(string.Empty, employee.MiddleName);
    }

    [Fact]
    public void Get_Full_Name_With_All_Of_Parts_Of_Name_Is_Filling()
    {
      Employee emp = new() { LastName = "LastName", FirstName = "FirstName", MiddleName = "MiddleName" };

      Assert.Equal("LastName FirstName MiddleName", emp.FullName);
    }

    [Fact]
    public void Get_Full_Name_With_All_Of_Parts_Of_Name_Is_Not_Filling()
    {
      Employee emp = new();

      Assert.Equal("DefaultLastName  ", emp.FullName);
    }

    [Fact]
    public void Get_Short_Name_With_All_Of_Parts_Of_Name_Is_Filling()
    {
      Employee emp = new() { LastName = "LastName", FirstName = "FirstName", MiddleName = "MiddleName" };

      Assert.Equal("LastName F.M.", emp.ShortName);
    }

    [Fact]
    public void Get_Short_Name_With_All_Of_Parts_Of_Name_Is_Not_Filling()
    {
      Employee emp = new();

      Assert.Equal("DefaultLastName ..", emp.ShortName);
    }

    [Fact]
    public void Add_Office_Equal_Null()
    {
      using var context = Fixture.CreateContext();
      var employee = context.Users.ToList().RandomOrDefault() ?? new Employee();

#pragma warning disable CS8625
      Assert.Throws<ArgumentNullException>(() => employee.AddOffice(null));
#pragma warning restore CS8625
    }

    [Fact]
    public void Add_Office_Not_Equal_Null()
    {
      using var context = Fixture.CreateContext();
      var employee = context.Users.ToList().RandomOrDefault() ?? new Employee();

      Office office = new Office();

      employee.AddOffice(office);

      Assert.Single(employee.Offices, o => o.Id == office.Id);
    }

    [Fact]
    public void Delete_Exist_Office()
    {
      using var context = Fixture.CreateContext();
      var employee = context.Users.ToList().RandomOrDefault() ?? new Employee();

      Guid idDeletingOffice = employee.Offices.LastOrDefault()?.Id ?? Guid.Empty;

      employee.DeleteOffice(idDeletingOffice);

      Assert.DoesNotContain(idDeletingOffice, employee.Offices.Select(o => o.Id));
    }

    [Fact]
    public void Delete_Not_Exist_Office()
    {
      using var context = Fixture.CreateContext();
      var employee = context.Users.ToList().RandomOrDefault() ?? new Employee();

      int officeCount = employee.Offices.Count;

      employee.DeleteOffice(Guid.NewGuid());

      Assert.Equal(officeCount, employee.Offices.Count);
    }
  }
}
