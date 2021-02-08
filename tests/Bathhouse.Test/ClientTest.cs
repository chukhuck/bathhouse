using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bathhouse.Test
{
  public class ClientTest
  {
    [Fact]
    public void Create_Client_With_Default_Values()
    {
      Client client = new();

      Assert.Equal("DefaultLastName", client.LastName);
      Assert.Equal(string.Empty, client.FirstName);
      Assert.Equal(string.Empty, client.MiddleName);
      Assert.Equal(string.Empty, client.Phone);
      Assert.Equal(ValueTypes.Sex.Unknown, client.Sex);
    }
  }
}
