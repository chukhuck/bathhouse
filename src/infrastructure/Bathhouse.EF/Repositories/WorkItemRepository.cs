using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class WorkItemRepository : Repository<WorkItem>, IWorkItemRepository
  {
    public WorkItemRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
