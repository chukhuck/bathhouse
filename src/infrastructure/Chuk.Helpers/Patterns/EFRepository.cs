using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Chuk.Helpers.Patterns
{
  public class EFRepository<TEntity, TEntityKey> : IRepository<TEntity, TEntityKey> 
    where TEntity : class, IEntity<TEntityKey>
    where TEntityKey : struct
  {
    protected DbContext context;

    public EFRepository(DbContext _context)
    {
      context = _context;
    }
    public TEntity Add(TEntity entity)
    {
      context.Set<TEntity>().Add(entity);
      return entity;
    }

    public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
    {
      context.Set<TEntity>().AddRange(entities);
      return entities;
    }

    public void Delete(TEntityKey id)
    {
      TEntity entity = context.Set<TEntity>().Find(id);
      context.Set<TEntity>().Remove(entity);
    }

    public void Delete(TEntity entity)
    {
      context.Set<TEntity>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntityKey> ids)
    {
      context.Set<TEntity>().RemoveRange(ids.Select(id => context.Set<TEntity>().Find(id)));
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
      context.Set<TEntity>().RemoveRange(entities);
    }

    public bool Exist(TEntityKey id)
    {
      return context.Set<TEntity>().Find(id) != null;
    }

    public TEntity? Get(
      TEntityKey key,
      IEnumerable<string>? includePropertyNames = null)
    {
      var entity = context.Set<TEntity>().AsNoTracking().FirstOrDefault(entity => entity.Id.Equals(key));

      if (entity is null)
        return null;

      var dBContextEntity = context.Attach(entity);

      if (includePropertyNames != null)
      {
        foreach (var includePropertyName in includePropertyNames)
        {
          dBContextEntity.Navigation(includePropertyName).Load();
        }
      }

      return entity;
    }


    public IEnumerable<TEntity> GetAll(
      PaginationFilter? paginationFilter = null,
      Expression<Func<TEntity, bool>>? filter = null, 
      IEnumerable<string>? includePropertyNames = null, 
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
      IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();

      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (includePropertyNames != null)
      {
        foreach (var includeExpression in includePropertyNames)
        {
          query = query.Include(includeExpression);
        }
      }

      if (paginationFilter != null && paginationFilter.PageNumber > 0 && paginationFilter.PageSize > 0)
      {
        int skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
        query = query.Skip(skip).Take(paginationFilter.PageSize);
      }


      if (orderBy != null)
      {
        return orderBy(query).ToList();
      }

      return query.ToList();
    }

    public IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
    {
      return context.Set<TEntity>().AsNoTracking().Where(predicate);
    }
  }
}
