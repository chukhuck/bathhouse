namespace Bathhouse.ValueTypes
{
  public struct SurveySummaryHeader
  {
    public DataType Type { get; set; }
    public string Text { get; set; }

    public static DataType ConvertType(QuestionType questionType)
    {
      return questionType switch
      {
        QuestionType.DateTime => DataType.DateTime,
        QuestionType.Decimal => DataType.Decimal,
        QuestionType.Number => DataType.Number,
        QuestionType.YesNo => DataType.Bool,
        QuestionType.Photo => DataType.Text,
        QuestionType.Text => DataType.Text,
        _ => DataType.Text
      };
    }
  }
}
