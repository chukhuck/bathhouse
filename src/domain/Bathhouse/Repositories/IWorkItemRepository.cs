using Bathhouse.Entities;
using System;

namespace Bathhouse.Repositories
{
  public interface IWorkItemRepository : IRepository<WorkItem, Guid>
  {
  }
}
