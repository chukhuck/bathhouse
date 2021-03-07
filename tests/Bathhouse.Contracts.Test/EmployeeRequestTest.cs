using Bathhouse.Contracts.Models.v1.Requests;
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
    public void Create_With_Default_Values()
    {
      EmployeeRequest emptyEmployee = new();

      Assert.Equal("Фамилия", emptyEmployee.LastName);
      Assert.Equal("Имя", emptyEmployee.FirstName);
      Assert.Equal("Отчество", emptyEmployee.MiddleName);
      Assert.Equal(DateTime.Parse("1950-01-01"), emptyEmployee.DoB);
      Assert.Equal("+7-495-000-00-00", emptyEmployee.PhoneNumber);
      Assert.Equal("noreply@mail.com", emptyEmployee.Email);
    }

    [Fact]
    public void Create_With_Empty_LastName_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.LastName = string.Empty;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field LastName is required.", new[] { "LastName" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_WhiteSpaces_LastName_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.LastName = new string(' ', 3);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field LastName is required.", new[] { "LastName" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Null_LastName_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.LastName = null;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field LastName is required.", new[] { "LastName" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Normal_LastName_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.LastName = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Incorrect_Phone_PhoneAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.PhoneNumber = "LastName";

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Incorrect phone format.", new[] { "PhoneNumber" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Correct_Phone_PhoneAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.PhoneNumber = "+7-916-000-0-000";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Incorrect_Email_EmailAttribute_Is_False()
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
    public void Create_With_Correct_Email_EmailAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Email = "test@test.com";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Lenght_LastName_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.LastName = new string('a', 26);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 25 symbols.", new[] { "LastName" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Lenght_LastName_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.LastName = new string('a', 25);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Lenght_FirstName_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.FirstName = new string('a', 26);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 25 symbols.", new[] { "FirstName" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Lenght_FirstName_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.FirstName = new string('a', 25);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Lenght_MiddleName_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.MiddleName = new string('a', 26);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 25 symbols.", new[] { "MiddleName" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Lenght_MiddleName_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.MiddleName = new string('a', 25);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }
  }
}
