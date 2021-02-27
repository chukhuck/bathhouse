using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Bathhouse.Api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
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
    [HttpGet(Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<IEnumerable<WorkItemResponse>> GetAll()
    {
      var allEntities = _repository.GetAll(includePropertyNames: new[] { "Creator", "Executor" });
      _logger.LogInformation($"All of WorkItems was got.");

      return Ok(_mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(allEntities));
    }

    /// <summary>
    /// Get WorkItem by ID
    /// </summary>
    /// <param name="workItemId">The WorkItem ID</param>
    [HttpGet("{workItemId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<WorkItemResponse> GetById(Guid workItemId)
    {
      WorkItem? entity = _repository.Get(key: workItemId, includePropertyNames: new[] { "Creator", "Executor" });
      if (entity is null)
      {
        _logger.LogInformation($"WorkItem with ID={workItemId} was not found.");
        return NotFound($"WorkItem with ID={workItemId} was not found.");
      }

      _logger.LogInformation($"WorkItem id={workItemId} was getting successfully.");
      return Ok(_mapper.Map<WorkItem, WorkItemResponse>(entity));
    }

    /// <summary>
    /// Add WorkItem.
    /// </summary>
    /// <param name="request">Newly creating WorkItem</param>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<WorkItemResponse> Create(WorkItemRequest request)
    {
      WorkItem newEntity = _repository.Add(_mapper.Map<WorkItemRequest, WorkItem>(request));

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={newEntity.Id} was creating successfully.");

      return CreatedAtAction(
        nameof(GetById),
        new { workItemId = newEntity.Id },
        _mapper.Map<WorkItem, WorkItemResponse>(newEntity));
    }

    /// <summary>
    /// Update WorkItem
    /// </summary>
    /// <param name="request">WorkItem for updating</param>
    /// <param name="workItemId">ID of WorkItem for updating</param>
    /// <returns></returns>
    [HttpPut("{workItemId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid workItemId, WorkItemRequest request)
    {
      WorkItem? entity = _repository.Get(key: workItemId, includePropertyNames: new[] { "Creator", "Executor" });
      if (entity is null)
      {
        _logger.LogInformation($"WorkItem with ID={workItemId} was not found.");
        return NotFound($"WorkItem with ID={workItemId} was not found.");
      }

      WorkItem updatedEntity = _mapper.Map<WorkItemRequest, WorkItem>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={workItemId}  was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete WorkItem by ID
    /// </summary>
    /// <param name="workItemId">WorkItem ID</param>
    [HttpDelete("{workItemId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid workItemId)
    {
      WorkItem? entity = _repository.Get(workItemId);
      if (entity is null)
      {
        _logger.LogInformation($"WorkItem with ID={workItemId} was not found.");
        return NotFound($"WorkItem with ID={workItemId} was not found.");
      }

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={workItemId} was deleted successfully.");

      return NoContent();

    }
    #endregion
  }
}
