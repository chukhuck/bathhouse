using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class EmployeeRepository : EFRepository<Employee, Guid>, IEmployeeRepository
  {
    public EmployeeRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
