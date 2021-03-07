using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.Models.Queries.v1
{
  public class WorkItemFilterQuery
  {
    public Guid? CreatorId { get; set; }
    public Guid? ExecutorId { get; set; }
    public WorkItemStatus? Status { get; set; }
  }
}
