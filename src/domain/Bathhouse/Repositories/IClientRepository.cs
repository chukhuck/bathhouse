using Bathhouse.Entities;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.Repositories
{
  public interface IClientRepository : IRepository<Client, Guid>
  {
  }
}
