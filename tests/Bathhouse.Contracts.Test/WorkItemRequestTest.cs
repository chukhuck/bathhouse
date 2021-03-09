﻿using Bathhouse.Contracts.v1.Models;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Contracts.Test
{
  public class WorkItemRequestTest
  {
    private readonly ValidationContext context;
    private readonly WorkItemRequest emptyRequest;

    public WorkItemRequestTest()
    {
      emptyRequest = new();
      context = new ValidationContext(emptyRequest);
    }

    [Fact]
    public void Create_With_Default_Values()
    {
      WorkItemRequest emptyWorkItem = new();

      Assert.Equal("Опиши текст задачи.", emptyWorkItem.Description);
      Assert.Equal(DateTime.Now.Date, emptyWorkItem.StartDate.Date);
      Assert.Equal(DateTime.Now.AddDays(1).Date, emptyWorkItem.EndDate.Date);
      Assert.Null(emptyWorkItem.ExecutorId);
      Assert.False(emptyWorkItem.IsImportant);
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
    public void Create_With_Empty_Description_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Description = string.Empty;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Description is required.", new[] { "Description" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_WhiteSpaces_Description_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Description = new string(' ', 3);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Description is required.", new[] { "Description" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Null_Description_RequiredAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Description = null;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Field Description is required.", new[] { "Description" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Normal_Description_RequiredAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Description = "LastName";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }
  }
}
