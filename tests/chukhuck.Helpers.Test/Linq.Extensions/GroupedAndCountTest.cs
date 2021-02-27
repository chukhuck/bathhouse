using Chuk.Helpers.Generators;
using Chuk.Helpers.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace chukhuck.Helpers.Test
{
  public class GroupedAndCountTest
  {
    [Fact]
    public void GroupedAndCount_NullSource_ThrowArgumentNullException()
    {
      List<int> source = null;

      Assert.Throws<ArgumentNullException>(() => source.GroupedAndCount());
    }

    [Fact]
    public void GroupedAndCount_EmptySource_EmptyCountedGroups()
    {
      List<int> source = new();

      var countedGroups = source.GroupedAndCount();

      Assert.Empty(countedGroups);
    }

    [Fact]
    public void GroupedAndCount_NotEmptySource_NotEmptyCountedGroups()
    {
      List<int> source = new() { 1, 2};

      var countedGroups = source.GroupedAndCount();

      Assert.NotEmpty(countedGroups);
    }

    [Fact]
    public void GroupedAndCount_EmptySource_NotNullCountedGroups()
    {
      List<int> source = new();

      var countedGroups = source.GroupedAndCount();

      Assert.NotNull(countedGroups);
    }

    [Fact]
    public void GroupedAndCount_NullableInt_NullAlsoCounted()
    {
      List<int?> source = new List<int?>() { 1, null, 2, 3};

      var countedGroups = source.GroupedAndCount();

      Assert.Equal(4, countedGroups.Count());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(15)]
    public void GroupedAndCount_NotNullableInt_Counted(int count)
    {
      var source = Sequence.Repeatable(maxValue: count);

      var countedGroups = source.GroupedAndCount();

      for (int i = 1; i <= count; i++)
      {
        Assert.Equal(i, countedGroups.Where(cg => cg.value == i).First().count);
      }
    }


    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(15)]
    public void GroupedAndCount_NotNullableRandomInt_SumCountInGroupsEqualTotalCount(int count)
    {
      var source = Sequence.Random(count: count * 2, minValue: 0, maxValue: count);

      var countedGroups = source.GroupedAndCount();

      int totalCount = countedGroups.Sum(cg => cg.count);
      Assert.Equal(count * 2, totalCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(15)]
    public void GroupedAndCount_NotNullableRandomInt_SumOfSourceEqualTotalSumOfValue(int count)
    {
      var source = Sequence.Random(count: count * 2, minValue: 0, maxValue: count).ToList();
      int sum = source.Sum();

      var countedGroups = source.GroupedAndCount();


      int totalValue = countedGroups.Sum(cg => cg.value * cg.count);
      Assert.Equal(sum, totalValue);
    }
  }
}
