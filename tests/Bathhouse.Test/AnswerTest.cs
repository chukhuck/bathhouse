using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bathhouse.Test
{
  public class AnswerTest
  {
    [Fact]
    public void Create_Answer_With_Value_Equal_String_Empty()
    {
      Answer answer = new();

      Assert.Equal(string.Empty, answer.Value);
    }
  }
}
