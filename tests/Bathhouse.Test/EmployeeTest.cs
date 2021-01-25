using Bathhouse.Entities;
using Bathhouse.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class EmployeeTest
  {
    public static readonly Employee employee = InMemoryContext.Employees.FirstOrDefault();


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
    public void Get_Offices_Return_Offices_Of_Employee()
    {
      IEnumerable<Office> offices = employee.GetOffices();

      Assert.Equal(employee.Offices, offices);
    }

    [Fact]
    public void Get_Offices_Return_Not_Null()
    {
      IEnumerable<Office> offices = employee.GetOffices();

      Assert.NotNull(offices);
    }

    [Fact]
    public void Get_Offices_Return_MyWorkItems_Of_Employee()
    {
      IEnumerable<WorkItem> workItems = employee.GetMyWorkItems();

      Assert.Equal(employee.WorkItems, workItems);
    }

    [Fact]
    public void Get_MyWorkItems_Return_Not_Null()
    {
      IEnumerable<WorkItem> workItems = employee.GetMyWorkItems();

      Assert.NotNull(workItems);
    }

    [Fact]
    public void Get_Offices_Return_CreatedWorkItems_Of_Employee()
    {
      IEnumerable<WorkItem> workItems = employee.GetCreatedWorkItems();

      Assert.Equal(employee.CreatedWorkItems, workItems);
    }

    [Fact]
    public void Get_CreatedWorkItems_Return_Not_Null()
    {
      IEnumerable<WorkItem> workItems = employee.GetCreatedWorkItems();

      Assert.NotNull(workItems);
    }

    [Fact]
    public void Get_Offices_Return_Surveys_Of_Employee()
    {
      IEnumerable<Survey> surveys = employee.GetSurveys();

      Assert.Equal(employee.Surveys, surveys);
    }

    [Fact]
    public void Get_Surveys_Return_Not_Null()
    {
      IEnumerable<Survey> surveys = employee.GetSurveys();

      Assert.NotNull(surveys);
    }

    [Fact]
    public void Add_Office_Equal_Null()
    {
      Assert.Throws<ArgumentNullException>(() => employee.AddOffice(null));
    }

    [Fact]
    public void Add_Office_Not_Equal_Null()
    {
      Office office = new Office();

      employee.AddOffice(office);

      Assert.Single(employee.GetOffices(), o => o.Id == office.Id);
    }

    [Fact]
    public void Delete_Exist_Office()
    {
      Guid idDeletingOffice = InMemoryContext.Employees.Where(e => e.Offices.Count != 0).LastOrDefault().Id;

      employee.DeleteOffice(idDeletingOffice);

      Assert.DoesNotContain(idDeletingOffice, employee.GetOffices().Select(o=>o.Id));
    }

    [Fact]
    public void Delete_Not_Exist_Office()
    {
      Employee emp = InMemoryContext.Employees.Where(e => e.Offices.Count != 0).LastOrDefault();
      int officeCount = emp.GetOffices().Count();

      employee.DeleteOffice(Guid.NewGuid());

      Assert.Equal(officeCount, emp.GetOffices().Count());
    }
  }
}
