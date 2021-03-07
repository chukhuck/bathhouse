using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.v1.Models.Queries
{
  public class WorkItemFilterQuery
  {
    public Guid? CreatorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public WorkItemStatus? Status { get; set; }
  }
}
