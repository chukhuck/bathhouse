using Microsoft.EntityFrameworkCore;

namespace Chuk.Helpers.Patterns
{
  public class EFUnitOfWork<TDBContext> : IUnitOfWork
    where TDBContext : DbContext
  {
    public EFUnitOfWork(TDBContext context)
    {
      Context = context;
    }
    public DbContext Context { get; }

    public int Complete()
    {
      return Context.SaveChanges();
    }

    public void Dispose()
    {
      Context.Dispose();
    }

    public virtual IRepository<TEntity, TEntityKey> Repository<TEntity, TEntityKey>()
      where TEntity : class, IEntity<TEntityKey>, new()
      where TEntityKey : struct
    {
      return new EFRepository<TEntity, TEntityKey>(Context);
    }
  }
}
