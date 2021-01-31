using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class ClientRepository : Repository<Client>, IClientRepository
  {
    public ClientRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
