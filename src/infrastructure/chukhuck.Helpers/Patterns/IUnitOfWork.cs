using System;

namespace chukhuck.Helpers.Patterns
{
  public interface IUnitOfWork : IDisposable
  {
    int Complete();

    IRepository<TEntity, TEntityKey> Repository<TEntity, TEntityKey>()
      where TEntity : class, IEntity<TEntityKey>, new()
      where TEntityKey : struct;
  }
}
