using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using System;
using Xunit;

namespace Bathhouse.Test
{
  public class WorkItemTest
  {
    [Fact]
    public void Create_Employee_With_Default_Values()
    {
      WorkItem workItem = new();

      Assert.False(workItem.IsImportant);
      Assert.False(workItem.IsUrgent);

      Assert.Equal(string.Empty, workItem.Description);
      Assert.Equal(WorkItemStatus.Created, workItem.Status);

      Assert.Equal(DateTime.Now.Date, workItem.CreationDate.Date);
      Assert.Equal(DateTime.Now.Date, workItem.StartDate.Date);
      Assert.Equal(DateTime.Now.AddDays(1).Date, workItem.EndDate.Date);
    }
  }
}
