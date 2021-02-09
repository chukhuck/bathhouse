using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using chukhuck.Helpers.Linq.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveyResultTest : IClassFixture<SharedBathhouseDbFixture>
  {
    public SurveyResultTest(SharedBathhouseDbFixture fixture) => Fixture = fixture;

    public SharedBathhouseDbFixture Fixture { get; }

    [Fact]
    public void To_List_With_No_Answers()
    {
      using var context = Fixture.CreateContext();
      var author = context.Users.ToList().RandomOrDefault() ?? new Employee();
      var survey = context.Surveys.ToList().RandomOrDefault() ?? new Survey();

      var surveyResult = new SurveyResult()
      {
        Answers = new List<Answer>(),
        Author = author,
        AuthorId = author.Id,
        Survey = survey,
        SurveyId = survey.Id
      };

      List<string> row = new();

      row.Add(surveyResult.CreationDate.ToString());
      row.Add(surveyResult.Author.LastName);

      Assert.Equal(row, surveyResult.ToList());
    }
  }
}
