using Bathhouse.EF.InMemory;
using Bathhouse.Entities;
using Chuk.Helpers.Linq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveyResultTest : IClassFixture<BathhouseDbFixture>
  {
    public SurveyResultTest(BathhouseDbFixture fixture) => Fixture = fixture;

    public BathhouseDbFixture Fixture { get; }

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
