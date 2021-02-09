using Bathhouse.Contracts.Models;
using Bathhouse.ValueTypes;
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
    public void Create_With_Default_Values()
    {
      QuestionRequest emptyQuestion = new();

      Assert.Equal("Текст нового вопроса", emptyQuestion.Text);
      Assert.Equal("Вопрос 1", emptyQuestion.Name);
      Assert.False(emptyQuestion.IsKey);
      Assert.Equal(QuestionType.Number, emptyQuestion.Type);
    }

    [Fact]
    public void Create_With_Incorrect_Type_EnumDataTypeAttribute_Is_False()
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
    public void Create_With_Correct_Sex_EnumDataTypeAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Type = QuestionType.Decimal;

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Lenght_Text_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
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
    public void Create_With_Lenght_Text_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = new string('a', 300);

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
    public void Create_With_Normal_Name_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Name = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Empty_Text_RequiredAttribute_Is_False()
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
    public void Create_With_Normal_Text_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_WhiteSpaces_Text_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = new string(' ', 3);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Text is required.", new[] { "Text" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Null_Text_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Text = null;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Text is required.", new[] { "Text" }),
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
  }
}
