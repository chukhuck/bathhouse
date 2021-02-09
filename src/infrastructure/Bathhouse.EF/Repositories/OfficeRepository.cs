using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class OfficeRepository : Repository<Office, Guid>, IOfficeRepository
  {
    public OfficeRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
