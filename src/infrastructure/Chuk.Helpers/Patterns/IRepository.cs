using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Chuk.Helpers.Patterns
{
  public interface IRepository<TEntity, TEntityKey> 
    where TEntity : class, IEntity<TEntityKey>
    where TEntityKey : struct
  {
    IEnumerable<TEntity> GetAll(
      PaginationFilter? paginationFilter = null,
      Expression<Func<TEntity, bool>>? filter = null,
      IEnumerable<string>? includePropertyNames = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

    TEntity? Get(TEntityKey key, IEnumerable<string> includePropertyNames = null);
    bool Exist(TEntityKey id);
    TEntity Add(TEntity entity);
    IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
    void Delete(TEntityKey id);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntityKey> ids);
    void DeleteRange(IEnumerable<TEntity> entities);
    IEnumerable<TEntity> Where(Func<TEntity, bool> predicate);
  }
}
