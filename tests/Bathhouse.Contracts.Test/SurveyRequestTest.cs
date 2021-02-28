using Bathhouse.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Contracts.Test
{
  public class SurveyRequestTest
  {
    private readonly ValidationContext context;
    private readonly SurveyRequest emptyRequest;

    public SurveyRequestTest()
    {
      emptyRequest = new();
      context = new ValidationContext(emptyRequest);
    }

    [Fact]
    public void Create_With_Default_Values()
    {
      SurveyRequest emptySurvey = new();

      Assert.Equal("Описание нового опроса", emptySurvey.Description);
      Assert.Equal("Новый опрос", emptySurvey.Name);
      Assert.NotNull(emptySurvey.Questions);
    }

    [Fact]
    public void Create_With_Lenght_Description_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Description = new string('a', 301);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 300 symbols.", new[] { "Description" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Lenght_Description_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Description = new string('a', 300);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Lenght_Name_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = new string('a', 51);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 50 symbols.", new[] { "Name" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Lenght_Name_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = new string('a', 50);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Empty_Name_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = string.Empty;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Name is required.", new[] { "Name" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_WhiteSpaces_Name_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = new string(' ', 3);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Name is required.", new[] { "Name" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Null_Name_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = null;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Name is required.", new[] { "Name" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Normal_Name_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }
  }
}
