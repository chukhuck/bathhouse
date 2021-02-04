using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Repositories
{
    #nullable enable
  public interface IRepository<TEntity> where TEntity : class
  {
    IEnumerable<TEntity> GetAll(
      Expression<Func<TEntity, bool>> filter = null,
      IEnumerable<string> navigationPropertyNames = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

    TEntity? Get<TEntityKey>(TEntityKey key, IEnumerable<string> navigationPropertyNames = null);
    bool Exist(Guid id);
    TEntity Add(TEntity entity);
    IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
    void Delete(Guid id);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<Guid> id);
    void DeleteRange(IEnumerable<TEntity> entities);
    IEnumerable<TEntity> Where(Func<TEntity, bool> predicate);
  }
}
