using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using Chuk.Helpers.Expressions;
using System;
using System.Linq.Expressions;

namespace Bathhouse.Contracts.Models
{
  public class WorkItemFilterQuery
  {
    public Guid? CreatorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public WorkItemStatus? Status { get; set; }

    public Expression<Func<WorkItem, bool>> Compose()
    {
      Expression < Func<WorkItem, bool>> filterByCreator = CreatorId == null ? wi => true : wi => wi.CreatorId == CreatorId;
      Expression < Func<WorkItem, bool>> filterByExecutor = ExecutorId == null ? wi => true : wi => wi.ExecutorId == ExecutorId;
      Expression < Func<WorkItem, bool>> filterByStatus = Status == null ? wi => true : wi => wi.Status == Status;

      return filterByCreator.And(filterByExecutor).And(filterByStatus);
    }
  }
}
