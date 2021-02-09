using System;
using System.Linq;
using System.Collections.Generic;

namespace chukhuck.Helpers.Linq.Extensions
{
  public static class RandomIEnumerableElementExtension
  {

    public static TElement? RandomOrDefault<TElement>(this ICollection<TElement> collection)
    {
      if (collection is null)
        throw new ArgumentNullException(nameof(collection));

      int count = collection.Count;

      if (count == 0)
        return default;

      Random randomiser = new Random();
      int index = randomiser.Next(minValue: 0, maxValue: count);
      return collection.ElementAt(index);
    }

    public static TElement? RandomOrDefault<TElement>(this IEnumerable<TElement> collection)
    {
      if (collection is null)
        throw new ArgumentNullException(nameof(collection));

      int count = collection.Count();

      if (count == 0)
        return default;

      Random randomiser = new Random();
      int index = randomiser.Next(minValue: 0, maxValue: count);
      return collection.ElementAt(index);
    }
  }
}
