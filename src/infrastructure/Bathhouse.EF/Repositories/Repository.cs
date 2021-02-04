﻿using Bathhouse.EF.Data;
using Bathhouse.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

    public bool Exist(Guid id)
    {
      return context.Set<TEntity>().Find(id) != null;
    }

    public TEntity? Get<TEntityKey>(
      TEntityKey key,
      IEnumerable<string>? navigationPropertyNames = null)
    {
      var entity = context.Set<TEntity>().Find(key);

      if (navigationPropertyNames != null)
      {
        foreach (var includePropertyName in navigationPropertyNames)
        {
          context.Entry(entity).Navigation(includePropertyName).Load();
        }
      }

      return entity;
    }


    public IEnumerable<TEntity> GetAll(
      Expression<Func<TEntity, bool>>? filter = null, 
      IEnumerable<string>? navigationPropertyNames = null, 
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
      IQueryable<TEntity> query = context.Set<TEntity>();

      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (navigationPropertyNames != null)
      {
        foreach (var includeExpression in navigationPropertyNames)
        {
          query = query.Include(includeExpression);
        }
      }

      if (orderBy != null)
      {
        return orderBy(query).AsNoTracking().ToList();
      }

      return query.AsNoTracking().ToList();
    }

    public IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
    {
      return context.Set<TEntity>().Where(predicate);
    }
  }
}
