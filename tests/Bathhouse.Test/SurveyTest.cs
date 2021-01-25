using Bathhouse.Entities;
using Bathhouse.Memory;
using Bathhouse.ValueTypes;
using System;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveyTest
  {
    public static readonly Survey survey = InMemoryContext.Surveys.FirstOrDefault();

    [Fact]
    public void GetSummury_With_Incorrect_Param()
    {
      Assert.Throws<ArgumentException>(() => survey.GetSummary( (SurveyResultSummaryType)int.MaxValue));
    }
  }
}
