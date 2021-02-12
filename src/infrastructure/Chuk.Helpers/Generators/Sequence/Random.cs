using System;
using System.Collections.Generic;

namespace Chuk.Helpers.Generators
{
  public static partial class Sequence
  {
    public static IEnumerable<int> Random(int count, int minValue = 0, int maxValue = int.MaxValue, int seed = 0)
    {
      Random generator = seed == 0 ? new() : new(seed);

      for (int i = 0; i < count; i++)
      {
        yield return generator.Next(minValue: minValue, maxValue: maxValue);
      }
    }
  }
}
