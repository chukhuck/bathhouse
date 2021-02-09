using Bathhouse.Entities;
using System;

namespace Bathhouse.Repositories
{
  public interface IEmployeeRepository : IRepository<Employee, Guid>
  {
  }
}
