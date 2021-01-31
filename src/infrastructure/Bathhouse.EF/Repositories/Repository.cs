using Bathhouse.EF.Data;
using Bathhouse.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.EF.Repositories
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    protected BathhouseContext context;

    public Repository(BathhouseContext _context)
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

    public void Delete(Guid id)
    {
      TEntity entity = context.Set<TEntity>().Find(id);
      context.Set<TEntity>().Remove(entity);
    }

    public void Delete(TEntity entity)
    {
      context.Set<TEntity>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<Guid> ids)
    {
      context.Set<TEntity>().RemoveRange(ids.Select(id => context.Set<TEntity>().Find(id)));
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
      context.Set<TEntity>().RemoveRange(entities);
    }

    public TEntity Get(Guid id)
    {
      return context.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
      return context.Set<TEntity>().AsNoTracking().ToList();
    }

    public IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
    {
      return context.Set<TEntity>().Where(predicate);
    }
  }
}
