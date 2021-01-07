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
  public class MemoryBaseCRUDRepository<TEntity> : ICRUDRepository<TEntity> where TEntity : Entity
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

    public virtual TEntity Get(Guid id)
    {
      if (!Exist(id))
        throw new ArgumentException("The resource is not found.");

      return entities.FirstOrDefault(o => o.Id == id);
    }

    public virtual TEntity Create(TEntity model)
    {
      entities.Add(model);
      return model;
    }

    public virtual TEntity Update(TEntity model)
    {
      if (!Exist(model.Id))
        throw new ArgumentException("The resource is not found.");

      entities.RemoveAll(entity => entity.Id == model.Id);
      entities.Add(model);

      return model;
    }

    public virtual void Delete(Guid id)
    {
      if (!Exist(id))
        throw new ArgumentException("The resource is not found.");

      entities.RemoveAll(entity => entity.Id == id);
    }

    public virtual bool Exist(Guid id)
    {
      return entities.Exists(entity => entity.Id == id);
    }
  }
}
