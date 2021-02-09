using Bathhouse.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Contracts.Test
{
  public class OfficeRequestTest
  {
    private readonly ValidationContext context;
    private readonly OfficeRequest emptyRequest;

    public OfficeRequestTest()
    {
      emptyRequest = new();
      context = new ValidationContext(emptyRequest);
    }

    [Fact]
    public void Create_EmployeeRequest_With_Default_Values()
    {
      OfficeRequest emptyOffice = new();

      Assert.Equal(0, emptyOffice.Number);
      Assert.Equal("Москва, ", emptyOffice.Address);
      Assert.Equal(DateTime.MinValue.AddHours(OfficeRequest.Hour_Of_Openning), emptyOffice.TimeOfOpen);
      Assert.Equal(DateTime.MinValue.AddHours(OfficeRequest.Hour_Of_Closing), emptyOffice.TimeOfClose);
      Assert.Equal("+7-495-000-00-00", emptyOffice.Phone);
      Assert.Equal("noreply@mail.com", emptyOffice.Email);
    }

    [Fact]
    public void EmployeeRequest_With_Incorrect_Phone_PhoneAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Phone = "LastName";

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Incorrect phone format.", new[] { "Phone" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void EmployeeRequest_With_Correct_Phone_PhoneAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Phone = "+7-916-000-0-000";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void EmployeeRequest_With_Incorrect_Email_EmailAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Email = "LastName";

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Incorrect email address format.", new[] { "Email" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void EmployeeRequest_With_Correct_Email_EmailAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Email = "test@test.com";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void EmployeeRequest_With_Lenght_Address_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Address = new string('a', 151);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 150 symbols.", new[] { "Address" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void EmployeeRequest_With_Lenght_Address_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Address = new string('a', 150);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }
  }
}
