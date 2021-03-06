﻿using Bathhouse.Api.Contracts.Models.Requests.v1;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Api.Contracts.Test
{
  public class ClientRequestTest
  {
    private readonly ValidationContext context;
    private readonly ClientRequest emptyRequest;

    public ClientRequestTest()
    {
      emptyRequest = new();
      context = new ValidationContext(emptyRequest);
    }


    [Fact]
    public void Create_With_Default_Values()
    {
      ClientRequest emptyClient = new();

      Assert.Equal("Фамилия", emptyClient.LastName);
      Assert.Equal("Имя", emptyClient.FirstName);
      Assert.Equal("Отчество", emptyClient.MiddleName);
      Assert.Equal(DateTime.Parse("1950 - 01 - 01"), emptyClient.DoB);
      Assert.Equal("+7-495-000-00-00", emptyClient.Phone);
      Assert.Equal("Комментарий", emptyClient.Comment);
      Assert.Equal(Sex.Unknown, emptyClient.Sex);
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

      emptyRequest.Phone = "LastName";

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Incorrect phone format.", new[] { "Phone" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Correct_Phone_PhoneAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Phone = "+7-916-000-0-000";

      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    [Fact]
    public void Create_With_Incorrect_Sex_EnumDataTypeAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Sex = (Sex)int.MaxValue;

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Incorrect a data type.", new[] { "Sex" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Correct_Sex_EnumDataTypeAttribute_Is_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Sex = Sex.Female;

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

    [Fact]
    public void Create_With_Lenght_Comment_More_Than_Max_Symbols_StringLengthAttribute_Is_False()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Comment = new string('a', 251);

      Assert.False(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Single(results);
      Assert.Contains(
        new ValidationResult("Maximum field length exceeded. Max lenght of field is 250 symbols.", new[] { "Comment" }),
        results,
        new ValidationResultEquilityComparer());
    }

    [Fact]
    public void Create_With_Lenght_Comment_Less_Than_Max_Symbols_StringLengthAttribute_True()
    {
      List<ValidationResult> results = new List<ValidationResult>();

      emptyRequest.Comment = new string('a', 250);
      Assert.True(Validator.TryValidateObject(emptyRequest, context, results, true));
      Assert.Empty(results);
    }

    // TODO Realize this test
    //[Fact]
    //public void Create_With_Incorrect_DoB_DataTypeAttribute_Is_False()
    //{
    //  ClientRequest emptyClient = new();
    //  PhoneAttribute attribute = new PhoneAttribute();
    //
    //  emptyClient.DoB = "LastName";
    //
    //  Assert.False(attribute.IsValid(emptyClient.DoB));
    //}
    //
    //[Fact]
    //public void Create_With_Correct_DoB_DataTypeAttribute_Is_True()
    //{
    //  ClientRequest emptyClient = new();
    //  PhoneAttribute attribute = new PhoneAttribute();
    //
    //  emptyClient.DoB = "+7-916-000-0-000";
    //
    //  Assert.True(attribute.IsValid(emptyClient.DoB));
    //}
  }
}
