using AutoMapper;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace Chuk.Helpers.AspNetCore
{
  [ApiController]
  public class CRUDControllerBase<TEntity, TEntityKey, TEntityResponse, TEntityRequest> : ControllerBase
    where TEntity : class, IEntity<TEntityKey>, new()
    where TEntityKey : struct
    where TEntityResponse : class
    where TEntityRequest : class
  {
    protected readonly IRepository<TEntity, TEntityKey> _repository;

    protected readonly ILogger<CRUDControllerBase<TEntity, TEntityKey, TEntityResponse, TEntityRequest>> _logger;

    protected readonly IMapper _mapper;

    protected readonly IUnitOfWork _unitOfWork;

    public CRUDControllerBase(
      ILogger<CRUDControllerBase<TEntity, TEntityKey, TEntityResponse, TEntityRequest>> logger,
      IMapper mapper,
      IUnitOfWork unitOfWork)
    {
      _logger = logger;
      _repository = unitOfWork.Repository<TEntity, TEntityKey>();
      _mapper = mapper;
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Get all of entities
    /// </summary>
    [HttpGet]
    public virtual ActionResult<PaginatedResponse<TEntityResponse>> GetAll([FromQuery] PaginationQuery paginationQuery)
    {
      PaginationFilter paginationFilter = new()
      {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber
      };

      var allEntities = _repository.GetAll(paginationFilter: paginationFilter);
      _logger.LogInformation($"All of entities was got.");

      return Ok(new PaginatedResponse<TEntityResponse>()
      {
        Data = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntityResponse>>(allEntities),
        PageNumber = paginationFilter.IsValid ? paginationFilter.PageNumber : null,
        PageSize = paginationFilter.IsValid ? paginationFilter.PageSize : null
      });
    }

    /// <summary>
    /// Get entity by ID
    /// </summary>
    /// <param name="id">The entity ID</param>
    [HttpGet("{id:guid}", Name = ("Get[controller]ById"))]
    public virtual ActionResult<TEntityResponse> GetById(TEntityKey id)
    {
      TEntity? entity = _repository.Get(id);
      if (entity is null)
      {
        _logger.LogInformation($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
        return NotFound($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
      }

      _logger.LogInformation($"Entity id={id} of type {typeof(TEntity)} was getting successfully.");
      return Ok(_mapper.Map<TEntity, TEntityResponse>(entity));
    }

    /// <summary>
    /// Create and add entity.
    /// </summary>
    /// <param name="request">Newly creating entity</param>
    [HttpPost(Name = ("Create[controller]"))]
    public virtual ActionResult<TEntityResponse> Create(TEntityRequest request)
    {
      TEntity newEntity = _repository.Add(_mapper.Map<TEntityRequest, TEntity>(request));

      _unitOfWork.Complete();
      _logger.LogInformation($"Entity id={newEntity.Id} of type {typeof(TEntity)} was creating successfully.");

      return CreatedAtAction("GetById", new { id = newEntity }, _mapper.Map<TEntity, TEntityResponse>(newEntity));
    }

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="request">Entity for updating</param>
    /// <param name="id">ID of entity for updating</param>
    [HttpPut("{id:guid}", Name = ("Update[controller]"))]
    public virtual ActionResult Update(TEntityKey id, TEntityRequest request)
    {
      TEntity? entity = _repository.Get(id);
      if (entity is null)
      {
        _logger.LogInformation($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
        return NotFound($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
      }

      _mapper.Map<TEntityRequest, TEntity>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Entity id={id} of type {typeof(TEntity)} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete entity by ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    [HttpDelete("{id:guid}", Name = ("Delete[controller]"))]
    public virtual IActionResult Delete(TEntityKey id)
    {
      TEntity? entity = _repository.Get(id);
      if (entity is null)
      {
        _logger.LogInformation($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
        return NotFound($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
      }

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Entity id={id} of type {typeof(TEntity)} was deleted successfully.");

      return NoContent();
    }
  }
}
