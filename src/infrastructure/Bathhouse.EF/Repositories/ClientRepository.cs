using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class ClientRepository : Repository<Client, Guid>, IClientRepository
  {
    public ClientRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
