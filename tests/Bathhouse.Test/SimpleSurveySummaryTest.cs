using Bathhouse.Entities;
using Bathhouse.Memory;
using Bathhouse.ValueTypes;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class SimpleSurveySummaryTest
  {
    public static readonly Survey survey = InMemoryContext.Surveys.LastOrDefault();

    [Fact]
    public void Get_Footers_Column_Count_Equal_1()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.Single(summary.Footers);
    }

    [Fact]
    public void Get_Headers_Column_Count_Equal_Question_Count_Plus_2()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.Equal(survey.Questions.Count + 2, summary.Headers.Count);
    }

    [Fact]
    public void Get_Data_Count_Equal_Survey_Result_Count()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.Equal(survey.Results.Count, summary.Data.Count);
    }

    [Fact]
    public void Get_Data_Column_Count_Equal_Question_Count_Plus_2()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      int x = survey.Questions.Count + 2;
      int y = summary.Data.FirstOrDefault().Count;
      Assert.Equal(x, y);
    }
  }
}
