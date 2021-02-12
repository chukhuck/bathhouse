using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class ClientRepository : EFRepository<Client, Guid>, IClientRepository
  {
    public ClientRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
