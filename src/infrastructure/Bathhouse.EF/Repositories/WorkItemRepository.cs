using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class WorkItemRepository : Repository<WorkItem, Guid>, IWorkItemRepository
  {
    public WorkItemRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
