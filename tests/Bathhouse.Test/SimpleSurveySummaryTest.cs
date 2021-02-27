using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using Chuk.Helpers.Linq;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class SimpleSurveySummaryTest : IClassFixture<BathhouseDbFixture>
  {
    public SimpleSurveySummaryTest(BathhouseDbFixture fixture) => Fixture = fixture;

    public BathhouseDbFixture Fixture { get; }


    [Fact]
    public void Get_Footers_Column_Count_Equal_1()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.Single(summary.Footers);
    }

    [Fact]
    public void Get_Headers_Column_Count_Equal_Question_Count_Plus_2()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.Equal(survey.Questions.Count + 2, summary.Headers.Count);
    }

    [Fact]
    public void Get_Data_Count_Equal_Survey_Result_Count()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.Equal(survey.Results.Count, summary.Data.Count);
    }

    [Fact]
    public void Get_Data_Column_Count_Equal_Question_Count_Plus_2()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      int x = survey.Questions.Count + 2;
      int y = summary.Data.FirstOrDefault()?.Count ?? 2;
      Assert.Equal(x, y);
    }
  }
}
