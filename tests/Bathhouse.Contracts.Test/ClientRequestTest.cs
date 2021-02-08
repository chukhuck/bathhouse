using Bathhouse.Contracts.Models;
using Bathhouse.ValueTypes;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Bathhouse.Contracts.Test
{
  public class ClientRequestTest
  {
    [Fact]
    public void Create_ClientRequest_With_Default_Values()
    {
      ClientRequest clientRequest = new();

      Assert.Equal("Фамилия", clientRequest.LastName);
      Assert.Equal("Имя", clientRequest.FirstName);
      Assert.Equal("Отчество", clientRequest.MiddleName);
      Assert.Equal(DateTime.Parse("1950 - 01 - 01"), clientRequest.DoB);
      Assert.Equal("+7-495-000-00-00", clientRequest.Phone);
      Assert.Equal("Комментарий", clientRequest.Comment);
      Assert.Equal(Sex.Unknown, clientRequest.Sex);
    }

    [Fact]
    public void ClientRequest_With_Empty_LastName_RequiredAttribute_Is_False()
    {
      ClientRequest clientRequest = new();
      RequiredAttribute attribute = new RequiredAttribute();

      clientRequest.LastName = string.Empty;

      Assert.False(attribute.IsValid(clientRequest.LastName));
    }

    [Fact]
    public void ClientRequest_With_Normal_LastName_RequiredAttribute_Is_True()
    {
      ClientRequest clientRequest = new();
      RequiredAttribute attribute = new RequiredAttribute();

      clientRequest.LastName = "LastName";

      Assert.True(attribute.IsValid(clientRequest.LastName));
    }

    [Fact]
    public void ClientRequest_With_Incorrect_Phone_PhoneAttribute_Is_False()
    {
      ClientRequest clientRequest = new();
      PhoneAttribute attribute = new PhoneAttribute();

      clientRequest.Phone = "LastName";

      Assert.False(attribute.IsValid(clientRequest.LastName));
    }

    [Fact]
    public void ClientRequest_With_Correct_Phone_PhoneAttribute_Is_True()
    {
      ClientRequest clientRequest = new();
      PhoneAttribute attribute = new PhoneAttribute();

      clientRequest.Phone = "+7-916-000-0-000";

      Assert.True(attribute.IsValid(clientRequest.Phone));
    }

    // TODO Realize this test
    //[Fact]
    //public void ClientRequest_With_Incorrect_DoB_DataTypeAttribute_Is_False()
    //{
    //  ClientRequest clientRequest = new();
    //  PhoneAttribute attribute = new PhoneAttribute();
    //
    //  clientRequest.DoB = "LastName";
    //
    //  Assert.False(attribute.IsValid(clientRequest.DoB));
    //}
    //
    //[Fact]
    //public void ClientRequest_With_Correct_DoB_DataTypeAttribute_Is_True()
    //{
    //  ClientRequest clientRequest = new();
    //  PhoneAttribute attribute = new PhoneAttribute();
    //
    //  clientRequest.DoB = "+7-916-000-0-000";
    //
    //  Assert.True(attribute.IsValid(clientRequest.DoB));
    //}



    [Fact]
    public void ClientRequest_With_Incorrect_Sex_EnumDataTypeAttribute_Is_False()
    {
      ClientRequest clientRequest = new();
      EnumDataTypeAttribute attribute = new EnumDataTypeAttribute(typeof(Sex));

      clientRequest.Sex = (Sex)int.MaxValue;

      Assert.False(attribute.IsValid(clientRequest.Sex));
    }

    [Fact]
    public void ClientRequest_With_Correct_Sex_EnumDataTypeAttribute_Is_True()
    {
      ClientRequest clientRequest = new();
      EnumDataTypeAttribute attribute = new EnumDataTypeAttribute(typeof(Sex));

      clientRequest.Sex = Sex.Female;

      Assert.True(attribute.IsValid(clientRequest.Sex));
    }
  }
}
