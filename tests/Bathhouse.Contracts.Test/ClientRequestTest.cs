using Bathhouse.Contracts.Models;
using Bathhouse.ValueTypes;
using System;
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
  }
}
