using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
  {
    public EmployeeRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
