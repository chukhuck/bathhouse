using Bathhouse.Entities;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.Repositories
{
  public interface IWorkItemRepository : IRepository<WorkItem, Guid>
  {
  }
}
