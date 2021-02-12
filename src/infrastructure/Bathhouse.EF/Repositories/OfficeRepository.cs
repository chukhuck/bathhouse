using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class OfficeRepository : EFRepository<Office, Guid>, IOfficeRepository
  {
    public OfficeRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
