using Bathhouse.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bathhouse.Memory.Repositories
{
  public class MemoryBaseCRUDRepository<TEntity> : ICRUDRepository<TEntity> where TEntity : class
  {
    protected List<TEntity> entities = new List<TEntity>();

    public MemoryBaseCRUDRepository()
    {
      entities = InMemoryContext.Init(entities);
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
      return entities;
    }

    public virtual TEntity? Get(Guid id)
    {
      return null;// entities.Find(o => o.Id == id);
    }

    public virtual TEntity Create(TEntity model)
    {
      entities.Add(model);
      return model;
    }

    public virtual TEntity Update(TEntity model)
    {
      //if (!Exist(model.Id))
      //  throw new ArgumentException($"The resource with ID={model.Id} is not found.");

      //entities.RemoveAll(entity => entity.Id == model.Id);
      entities.Add(model);

      return model;
    }

    public virtual void Delete(Guid id)
    {
      if (!Exist(id))
        throw new ArgumentException("The resource with ID={model.Id} is not found.");

      //entities.RemoveAll(entity => entity.Id == id);
    }

    public virtual bool Exist(Guid id)
    {
      return true;// entities.Exists(entity => entity.Id == id);
    }

    public virtual bool SaveChanges()
    {
      try
      {
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception(message: "The error occured when changes was saving.", innerException: ex);
      }

    }

    public virtual IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
    {
      return entities.Where(predicate);
    }
  }
}
