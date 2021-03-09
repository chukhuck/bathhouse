﻿using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace Bathhouse.Api.Controllers
{
  [ApiController]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public class RichControllerBase<TEntity, TEntityKey, TEntityResponse, TEntityRequest> : ControllerBase
    where TEntity : class, IEntity<TEntityKey>, new()
    where TEntityKey : struct
    where TEntityResponse : class
    where TEntityRequest : class
  {
    protected readonly IRepository<TEntity, TEntityKey> _repository;

    protected readonly ILogger<RichControllerBase<TEntity, TEntityKey, TEntityResponse, TEntityRequest>> _logger;

    protected readonly IMapper _mapper;

    protected readonly IUnitOfWork _unitOfWork;

    public RichControllerBase(
      ILogger<RichControllerBase<TEntity, TEntityKey, TEntityResponse, TEntityRequest>> logger,
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
    /// <response code="200">Getting all of entities was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual ActionResult<IEnumerable<TEntityResponse>> Get()
    {
      try
      {
        var allEntities = _repository.GetAll();
        _logger.LogInformation($"All of entities was got.");

        return Ok(_mapper.Map<IEnumerable<TEntity>, IEnumerable<TEntityResponse>>(allEntities));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of entities an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of entities an exception was fired.");
      }
    }

    /// <summary>
    /// Get entity by ID
    /// </summary>
    /// <param name="id">The entity ID</param>
    /// <response code="404">Entity with current ID is not found</response>
    /// <response code="200">Getting entity is successul</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual ActionResult<TEntityResponse> GetById(TEntityKey id)
    {
      try
      {
        if (_repository.Get(id) is TEntity entity)
        {
          _logger.LogInformation($"Entity id={id} of type {typeof(TEntity)} was getting successfully.");
          return Ok(_mapper.Map<TEntity, TEntityResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting entity id={id} of type {typeof(TEntity)} was received.");
          return NotFound($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting entity id={id} of type {typeof(TEntity)} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting entity id={id} of type {typeof(TEntity)} an exception was fired");
      }
    }

    /// <summary>
    /// Create and add entity.
    /// </summary>
    /// <param name="request">Newly creating entity</param>
    /// <response code="201">Creating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual ActionResult<TEntityResponse> Create(TEntityRequest request)
    {
      try
      {
        TEntity newEntity = _repository.Add(_mapper.Map<TEntityRequest, TEntity>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"Entity id= of type {typeof(TEntity)} was creating successfully.");

        return CreatedAtAction("GetById", new { id = newEntity }, _mapper.Map<TEntity, TEntityResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating entity of type {typeof(TEntity)} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating entity of type {typeof(TEntity)} an exception was fired");
      }
    }

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="request">Entity for updating</param>
    /// <param name="id">ID of entity for updating</param>
    /// <response code="204">Updating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Entity with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual ActionResult Update(TEntityKey id, TEntityRequest request)
    {
      try
      {
        if (_repository.Get(id) is TEntity entity)
        {
          TEntity updatedEntity = _mapper.Map<TEntityRequest, TEntity>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"Entity id={id} of type {typeof(TEntity)} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
          return NotFound($"Entity with ID={id} of type {typeof(TEntity)} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating entity of type {typeof(TEntity)} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating entity of type {typeof(TEntity)} an exception was fired");
      }
    }

    /// <summary>
    /// Delete entity by ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <response code="404">Entity with current ID is not found</response>
    /// <response code="204">Deleting entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual IActionResult Delete(TEntityKey id)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While deleting entity id={id} of type {typeof(TEntity)} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting entity id={id} of type {typeof(TEntity)} an exception was fired");
      }
    }
  }
}
