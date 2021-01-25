using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using System;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveySummaryFactoryTest
  {
    [Fact]
    public void Survey_Summary_Factory_Create_With_Simple_Type()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Simple);

      Assert.IsType<SimpleSurveySummary>(summary);
    }

    [Fact]
    public void Survey_Summary_Factory_Create_With_Aggregated_Type()
    {
      Survey survey = new Survey();

      SurveySummary summary = SurveySummaryFactory.Create(survey, SurveyResultSummaryType.Aggregated);

      Assert.IsType<AggregatedSurveySummary>(summary);
    }

    [Fact]
    public void Survey_Summary_Factory_Create_With_Incorrect_Param()
    {
      Assert.Throws<ArgumentException>(() => SurveySummaryFactory.Create(new Survey(), (SurveyResultSummaryType)int.MaxValue));
    }
  }
}
