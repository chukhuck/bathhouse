using Bathhouse.Contracts.Models;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Contracts.Test
{
  public class EmployeeRequestTest
  {
    private readonly ValidationContext context;
    private readonly EmployeeRequest emptyRequest;

    public EmployeeRequestTest()
    {
      emptyRequest = new();
      context = new ValidationContext(emptyRequest);
    }

    [Fact]
    public void Create_EmployeeRequest_With_Default_Values()
    {
      EmployeeRequest emptyEmployee = new();

      Assert.Equal("Фамилия", emptyEmployee.LastName);
      Assert.Equal("Имя", emptyEmployee.FirstName);
      Assert.Equal("Отчество", emptyEmployee.MiddleName);
      Assert.Equal(DateTime.Parse("1950-01-01"), emptyEmployee.DoB);
      Assert.Equal("+7-495-000-00-00", emptyEmployee.PhoneNumber);
      Assert.Equal("noreply@mail.com", emptyEmployee.Email);
    }
  }
}
