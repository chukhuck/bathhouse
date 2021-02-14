using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class RoleRepository : EFRepository<Role, Guid>, IRoleRepository
  {
    public RoleRepository(BathhouseContext _context) : base(_context)
  {
  }
}
}
