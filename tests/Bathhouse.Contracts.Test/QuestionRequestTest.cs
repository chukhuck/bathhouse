using Bathhouse.Contracts.Models;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Contracts.Test
{
  public class QuestionRequestTest
  {
    private readonly ValidationContext context;
    private readonly QuestionRequest emptyRequest;

    public QuestionRequestTest()
    {
      emptyRequest = new();
      context = new ValidationContext(emptyRequest);
    }

    [Fact]
    public void Create_QuestionRequest_With_Default_Values()
    {
      QuestionRequest emptyEmployee = new();

      Assert.Equal("Текст нового вопроса", emptyEmployee.Text);
      Assert.Equal("Вопрос 1", emptyEmployee.Name);
      Assert.False(emptyEmployee.IsKey);
      Assert.Equal(QuestionType.Number, emptyEmployee.Type);
    }

    [Fact]
    public void ClientRequest_With_Incorrect_Type_EnumDataTypeAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Type = (QuestionType)int.MaxValue;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Incorrect a data type.", new[] { "Type" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void ClientRequest_With_Correct_Sex_EnumDataTypeAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Type = QuestionType.Decimal;

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void QuestionRequest_With_Lenght_Text_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = new string('a', 301);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 300 symbols.", new[] { "Text" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void QuestionRequest_With_Lenght_LastName_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = new string('a', 300);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void QuestionRequest_With_Lenght_FirstName_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
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
    public void QuestionRequest_With_Lenght_FirstName_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = new string('a', 50);

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void QuestionRequest_With_Empty_Name_RequiredAttribute_Is_False()
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
    public void QuestionRequest_With_Normal_Name_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void QuestionRequest_With_Empty_Text_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = string.Empty;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Text is required.", new[] { "Text" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void QuestionRequest_With_Normal_Text_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }
  }
}
