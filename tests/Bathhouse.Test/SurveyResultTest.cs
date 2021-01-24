using System.Linq;
using Bathhouse.Entities;
using Bathhouse.Memory;
using System.Collections.Generic;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveyResultTest
  {
    public static readonly SurveyResult surveyResult = InMemoryContext.SurveyResults.FirstOrDefault();

    [Fact]
    public void To_List_With_No_Answers()
    {
      surveyResult.Answers = new List<Answer>();

      List<string> row = new List<string>();

      row.Add(surveyResult.CreationDate.ToString());
      row.Add(surveyResult.Author.LastName);

      Assert.Equal(row, surveyResult.ToList());
    }
  }
}
