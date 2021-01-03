using AutoMapper;
using Bathhouse.Models;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Http;
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
    where TEntityModel : EntityModel  
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

    /// <summary>
    /// Get all of entities
    /// </summary>
    /// <returns>Entities</returns>
    [HttpGet]
    public IEnumerable<TEntityModel> Get()
    {
      return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntityModel>>(_repository.GetAll());
    }

    /// <summary>
    /// Get entity by ID
    /// </summary>
    /// <param name="id">The entity ID</param>
    /// <returns>The finding entity</returns>
    [HttpGet]
    [Route("{id:guid}")]
    public ActionResult<TEntityModel> GetById(Guid id)
    {
      return Ok(_mapper.Map<TEntity, TEntityModel>(_repository.Get(id)));
    }

    /// <summary>
    /// Create entiry and add it.
    /// </summary>
    /// <param name="entityModel">Newly creating entity</param>
    /// <returns>Newly created entity</returns>
    [HttpPost]
    public ActionResult<TEntityModel> Create(TEntityModel entityModel)
    {
      TEntity newEntity = _repository.Create(_mapper.Map<TEntityModel, TEntity>(entityModel));

      return Ok(_mapper.Map<TEntity, TEntityModel>(newEntity));

    }

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="entity">Entity for updating</param>
    /// <response code="400">If the item is null</response>
    /// <returns></returns>
    [HttpPut]
    public ActionResult Update(TEntityModel entity)
    {
      TEntity updatedEntity = _repository.Update(_mapper.Map<TEntityModel, TEntity>(entity));

      return Ok(_mapper.Map<TEntity, TEntityModel>(updatedEntity));
    }

    /// <summary>
    /// Delete entity by ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <response code="404">Entity with current ID is not found</response>
    /// <response code="204">Deleting entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
    [ProducesResponseType((int)StatusCodes.Status404NotFound)]
    [ProducesResponseType((int)StatusCodes.Status500InternalServerError)]
    public IActionResult Delete(Guid id)
    {
      try
      {
        if (!_repository.Exist(id))
        {
          _logger.LogInformation($"Request on deleting unexisting entity id={id} of type {typeof(TEntity)} was received.");
          return NotFound();
        }

        _repository.Delete(id);
        _logger.LogInformation($"Entity id={id} of type {typeof(TEntity)} was deleted successfully.");
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting entity id={id} of type {typeof(TEntity)} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting entity id={id} of type {typeof(TEntity)} an exception was fired");
      }

      return NoContent();
    }
  }
}
