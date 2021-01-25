using Bathhouse.Entities;
using Bathhouse.Memory;
using Bathhouse.ValueTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class AggregatedSurveySummaryTest
  {
    public static readonly Survey survey = InMemoryContext.Surveys.LastOrDefault();

    [Fact]
    public void Get_Footers_Column_Count_Equal_Question_Count_Plus_2()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Questions.Count + 2, summary.Footers.Count);
    }

    [Fact]
    public void Get_Headers_Column_Count_Equal_Question_Count_Plus_2()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Questions.Count + 2, summary.Headers.Count);
    }

    [Fact]
    public void Get_Data_Count_Equal_Survey_Result_Count()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Results.Count, summary.Data.Count);
    }

    [Fact]
    public void Get_Data_Column_Count_Equal_Question_Count_Plus_2()
    {
      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Questions.Count + 2, summary.Data.FirstOrDefault().Count);
    }
  }
}
