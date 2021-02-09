using Bathhouse.Entities;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.Repositories
{
  public interface IEmployeeRepository : IRepository<Employee, Guid>
  {
  }
}
