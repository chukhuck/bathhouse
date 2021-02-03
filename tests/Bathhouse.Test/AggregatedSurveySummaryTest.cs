using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using chukhuck.Linq.Extensions;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class AggregatedSurveySummaryTest : IClassFixture<SharedBathhouseDbFixture>
  {
    public AggregatedSurveySummaryTest(SharedBathhouseDbFixture fixture) => Fixture = fixture;

    public SharedBathhouseDbFixture Fixture { get; }

    [Fact]
    public void Get_Footers_Column_Count_Equal_Question_Count_Plus_2()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Questions.Count + 2, summary.Footers.Count);

    }

    [Fact]
    public void Get_Headers_Column_Count_Equal_Question_Count_Plus_2()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Questions.Count + 2, summary.Headers.Count);
    }

    [Fact]
    public void Get_Data_Count_Equal_Survey_Result_Count()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.Equal(survey.Results.Count, summary.Data.Count);
    }

    [Fact]
    public void Get_Data_Column_Count_Equal_Question_Count_Plus_2()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Aggregated);

      var row = summary.Data.FirstOrDefault();

      if (row is not null)
      {
        Assert.Equal(survey.Questions.Count + 2, row.Count);
      }
    }
  }
}
