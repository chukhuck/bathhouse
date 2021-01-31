using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class OfficeRepository : Repository<Office>, IOfficeRepository
  {
    public OfficeRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
