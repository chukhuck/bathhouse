using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using chukhuck.Linq.Extensions;
using System;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveyTest : IClassFixture<SharedBathhouseDbFixture>
  {
    public SurveyTest(SharedBathhouseDbFixture fixture) => Fixture = fixture;

    public SharedBathhouseDbFixture Fixture { get; }


    [Fact]
    public void Create_Survey_With_Default_Values()
    {
      Survey survey = new();

      Assert.Equal("New survey", survey.Name);
      Assert.Equal(SurveyStatus.Work, survey.Status);
      Assert.Equal(DateTime.Now.Date, survey.CreationDate.Date);
    }

    [Fact]
    public void GetSummury_With_Incorrect_Param()
    {
      using var context = Fixture.CreateContext();
      var survey = context.Surveys.Local.RandomOrDefault() ?? new Survey();

      Assert.Throws<ArgumentException>(() => survey.GetSummary((SurveyResultSummaryType)int.MaxValue));
    }
  }
}
