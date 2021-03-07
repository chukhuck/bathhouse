using Bathhouse.Contracts.Models.Queries.v1;
using Bathhouse.Entities;
using Chuk.Helpers.Expressions;
using System;
using System.Linq.Expressions;

namespace Bathhouse.Api.Common.Filters
{
  public class WorkItemFilter
  {
    private readonly WorkItemFilterQuery query;

    public WorkItemFilter(WorkItemFilterQuery query)
    {
      this.query = query;
    }

    public Expression<Func<WorkItem, bool>> Compose()
    {
      Expression<Func<WorkItem, bool>> filterByCreator = query?.CreatorId == null ? wi => true : wi => wi.CreatorId == query.CreatorId;
      Expression<Func<WorkItem, bool>> filterByExecutor = query?.ExecutorId == null ? wi => true : wi => wi.ExecutorId == query.ExecutorId;
      Expression<Func<WorkItem, bool>> filterByStatus = query?.Status == null ? wi => true : wi => wi.Status == query.Status;

      return filterByCreator.And(filterByExecutor).And(filterByStatus);
    }
  }
}
