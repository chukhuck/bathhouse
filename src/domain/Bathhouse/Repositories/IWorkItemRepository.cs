using Bathhouse.Entities;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.Repositories
{
  public interface IWorkItemRepository : IRepository<WorkItem, Guid>
  {
  }
}
