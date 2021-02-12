using System.Collections.Generic;

namespace Chuk.Helpers.Generators
{
  public static partial class Sequence
  {
    public static IEnumerable<int> Repeatable(int maxValue)
    {
      int countOfItems = ((1 + maxValue) * maxValue) / 2;
      List<int> source = new(countOfItems);

      for (int i = 1; i <= maxValue; i++)
      {
        for (int j = 0; j < i; j++)
        {
          source.Add(i);
        }
      }

      return source;
    }
  }
}
