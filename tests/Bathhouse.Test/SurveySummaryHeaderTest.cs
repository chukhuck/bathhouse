using Bathhouse.ValueTypes;
using Xunit;

namespace Bathhouse.Test
{
  public class SurveySummaryHeaderTest
  {
    [Fact]
    public void Survey_Summary_Header_Convert_DateTime()
    {
      DataType type = SurveySummaryHeader.ConvertType(QuestionType.DateTime);

      Assert.Equal(DataType.DateTime, type);
    }

    [Fact]
    public void Survey_Summary_Header_Convert_Decimal()
    {
      DataType type = SurveySummaryHeader.ConvertType(QuestionType.Decimal);

      Assert.Equal(DataType.Decimal, type);
    }

    [Fact]
    public void Survey_Summary_Header_Convert_Number()
    {
      DataType type = SurveySummaryHeader.ConvertType(QuestionType.Number);

      Assert.Equal(DataType.Number, type);
    }

    [Fact]
    public void Survey_Summary_Header_Convert_YesNo()
    {
      DataType type = SurveySummaryHeader.ConvertType(QuestionType.YesNo);

      Assert.Equal(DataType.Bool, type);
    }

    [Fact]
    public void Survey_Summary_Header_Convert_Photo()
    {
      DataType type = SurveySummaryHeader.ConvertType(QuestionType.Photo);

      Assert.Equal(DataType.Text, type);
    }

    [Fact]
    public void Survey_Summary_Header_Convert_Text()
    {
      DataType type = SurveySummaryHeader.ConvertType(QuestionType.Text);

      Assert.Equal(DataType.Text, type);
    }

    [Fact]
    public void Survey_Summary_Header_Convert_Incorrect_Param()
    {
      DataType type = SurveySummaryHeader.ConvertType((QuestionType)int.MaxValue);

      Assert.Equal(DataType.Text, type);
    }
  }
}
