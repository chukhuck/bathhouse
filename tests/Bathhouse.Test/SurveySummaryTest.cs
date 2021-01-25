using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using System;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveySummaryTest
  {
    [Fact]
    public void New_Simple_SurveySummary_With_Null_Survey_Param()
    {
      Assert.Throws<ArgumentNullException>(() => new SimpleSurveySummary(null));
    }

    [Fact]
    public void New_Aggregated_SurveySummary_With_Null_Survey_Param()
    {
      Assert.Throws<ArgumentNullException>(() => new AggregatedSurveySummary(null));
    }

    [Fact]
    public void Survey_Summary_Create_With_Simple_Type()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummary.Create(survey, SurveyResultSummaryType.Simple);

      Assert.IsType<SimpleSurveySummary>(summary);
    }

    [Fact]
    public void Survey_Summary_Create_With_Aggregated_Type()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.IsType<AggregatedSurveySummary>(summary);
    }

    [Fact]
    public void Survey_Summary_After_Create_Headers_Is_Not_Null()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Simple);

      Assert.NotNull(summary.Headers);
    }

    [Fact]
    public void Survey_Summary_After_Create_Footers_Is_Not_Null()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Simple);

      Assert.NotNull(summary.Footers);
    }

    [Fact]
    public void Survey_Summary_After_Create_Data_Is_Not_Null()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Simple);

      Assert.NotNull(summary.Data);
    }

    [Fact]
    public void Survey_Summary_After_Create_Survey_Is_Not_Null()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Simple);

      Assert.NotNull(summary.Survey);
    }
  }
}
