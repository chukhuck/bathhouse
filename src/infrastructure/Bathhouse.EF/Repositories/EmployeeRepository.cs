using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class EmployeeRepository : Repository<Employee, Guid>, IEmployeeRepository
  {
    public EmployeeRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
