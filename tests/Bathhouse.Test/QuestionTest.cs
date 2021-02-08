using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using System;
using Xunit;

namespace Bathhouse.Test
{
  public class QuestionTest
  {
    [Fact]
    public void Create_Question_With_Default_Values()
    {
      Question question = new();

      Assert.False(question.IsKey);
      Assert.Equal("New question", question.Text);
      Assert.Equal("Newquestion", question.Name);
      Assert.Equal(QuestionType.Number, question.Type);
    }
  }
}
