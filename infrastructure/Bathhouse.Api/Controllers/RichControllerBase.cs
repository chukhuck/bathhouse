using AutoMapper;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Bathhouse.Api.Controllers
{

  [ApiController]
  public class RichControllerBase<TEntity, TEntityModel> : ControllerBase
    where TEntity: Entity 
    where TEntityModel : class  
  {
    protected readonly ICRUDRepository<TEntity> _repository;

    protected readonly ILogger<RichControllerBase<TEntity, TEntityModel>> _logger;

    protected readonly IMapper _mapper;

    public RichControllerBase(ILogger<RichControllerBase<TEntity, TEntityModel>> logger, IMapper mapper, ICRUDRepository<TEntity> repository)
    {
      _logger = logger;
      _repository = repository;
      _mapper = mapper;
      
    }

    [HttpGet]
    public IEnumerable<TEntityModel> Get()
    {
      return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntityModel>>(_repository.GetAll());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public TEntityModel GetById(Guid id)
    {
      return _mapper.Map<TEntity, TEntityModel>(_repository.Get(id));
    }

    [HttpPost]
    public IActionResult Add(TEntityModel office)
    {
      _repository.Create(_mapper.Map<TEntityModel, TEntity>(office));

      return Ok();
    }

    [HttpPut]
    public IActionResult Update(TEntityModel office)
    {
      _repository.Update(_mapper.Map<TEntityModel, TEntity>(office));

      return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
      _repository.Delete(id);

      return Ok();
    }
  }
}
