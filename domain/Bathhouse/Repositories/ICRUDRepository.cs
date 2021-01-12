﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Repositories
{
  public interface ICRUDRepository<TEntity> where TEntity : Entity
  {
    public IEnumerable<TEntity> GetAll();
    public TEntity Get(Guid id);
    public TEntity Create(TEntity model);
    public TEntity Update(TEntity model);
    public void Delete(Guid id);
    public bool Exist(Guid id);
    public bool SaveChanges();
  }
}
