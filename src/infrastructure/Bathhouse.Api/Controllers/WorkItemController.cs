using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using chukhuck.Helpers.Patterns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class WorkItemController : ControllerBase
  {

    protected readonly IBathhouseUnitOfWork _unitOfWork;
    protected readonly IRepository<WorkItem, Guid> _repository;
    protected readonly ILogger<WorkItemController> _logger;
    protected readonly IMapper _mapper;

    public WorkItemController(ILogger<WorkItemController> logger, IMapper mapper, IBathhouseUnitOfWork unitOfWork)
    {
      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<WorkItem, Guid>();
    }

    #region CRUD endpoints
    /// <summary>
    /// Get all of WorkItems
    /// </summary>
    /// <response code="200">Getting all of WorkItems was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual ActionResult<IEnumerable<WorkItemResponse>> Get()
    {
      try
      {
        var allEntities = _repository.GetAll(includePropertyNames: new[] { "Creator", "Executor" });
        _logger.LogInformation($"All of WorkItems was got.");

        return Ok(_mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(allEntities));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of WorkItems an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of WorkItems an exception was fired.");
      }
    }

    /// <summary>
    /// Get WorkItem by ID
    /// </summary>
    /// <param name="id">The WorkItem ID</param>
    /// <response code="404">WorkItem with current ID is not found</response>
    /// <response code="200">Getting entity is successul</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<WorkItemResponse> GetById(Guid id)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Creator", "Executor"}) is WorkItem entity)
        {
          _logger.LogInformation($"WorkItem id={id} was getting successfully.");
          return Ok(_mapper.Map<WorkItem, WorkItemResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting WorkItem id={id} was received.");
          return NotFound($"WorkItem with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting WorkItem id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting WorkItem id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Add WorkItem.
    /// </summary>
    /// <param name="request">Newly creating WorkItem</param>
    /// <response code="201">Creating WorkItem is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<WorkItemResponse> Create(WorkItemRequest request)
    {
      try
      {
        WorkItem newEntity = _repository.Add(_mapper.Map<WorkItemRequest, WorkItem>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={newEntity.Id} was creating successfully.");

        return CreatedAtAction("GetById", new { id = newEntity.Id }, _mapper.Map<WorkItem, WorkItemResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating WorkItem an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating WorkItem an exception was fired");
      }
    }

    /// <summary>
    /// Update WorkItem
    /// </summary>
    /// <param name="request">WorkItem for updating</param>
    /// <param name="id">ID of WorkItem for updating</param>
    /// <response code="204">Updating WorkItem is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Entity with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult Update(Guid id, WorkItemRequest request)
    {
      try
      {
        if (_repository.Get(id) is WorkItem entity)
        {
          WorkItem updatedEntity = _mapper.Map<WorkItemRequest, WorkItem>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"WorkItem id={id}  was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"WorkItem with ID={id} was not found.");
          return NotFound($"WorkItem with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating WorkItem an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating WorkItem an exception was fired");
      }
    }

    /// <summary>
    /// Delete WorkItem by ID
    /// </summary>
    /// <param name="id">WorkItem ID</param>
    /// <response code="404">WorkItem with current ID is not found</response>
    /// <response code="204">Deleting WorkItem is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual IActionResult Delete(Guid id)
    {
      try
      {
        WorkItem? entity = _repository.Get(id);
        if (entity is null)
        {
          _logger.LogInformation($"WorkItem with ID={id} was not found.");
          return NotFound($"WorkItem with ID={id} was not found.");
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={id} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting WorkItem id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting WorkItem id={id} an exception was fired");
      }
    }
    #endregion
  }
}
