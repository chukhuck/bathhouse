using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class WorkItemRepository : EFRepository<WorkItem, Guid>, IWorkItemRepository
  {
    public WorkItemRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
