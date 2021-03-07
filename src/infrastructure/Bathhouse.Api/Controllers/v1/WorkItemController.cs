using AutoMapper;
using Bathhouse.Contracts.v1.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace Bathhouse.Api.v1.Controllers
{
  [Authorize]
  [ApiController]
  [ApiVersion("1.0")]
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
    [HttpGet("[controller]s", Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<WorkItemResponse>> GetAll(
      [FromQuery] PaginationQuery paginationQuery,
      [FromQuery] WorkItemFilterQuery filter)
    {
      PaginationFilter paginationFilter = new()
      {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber
      };

      var allEntities = _repository.GetAll(
        paginationFilter: paginationFilter, 
        filter: filter.Compose(),
        includePropertyNames: new[] { "Creator", "Executor" },
        orderBy: all => all.OrderBy(c => c.CreationDate));

      _logger.LogInformation($"All of WorkItems was got.");

      return Ok(new PaginatedResponse<WorkItemResponse>()
      {
        Data = _mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(allEntities),
        PageNumber = paginationFilter.IsValid ? paginationFilter.PageNumber : null,
        PageSize = paginationFilter.IsValid ? paginationFilter.PageSize : null
      });
    }

    /// <summary>
    /// Get WorkItem by ID
    /// </summary>
    /// <param name="workItemId">The WorkItem ID</param>
    [HttpGet("[controller]s/{workItemId:guid}", Name = ("Get[controller]ById"))]
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
    [HttpPost("[controller]s", Name = ("Create[controller]"))]
    public ActionResult<WorkItemResponse> Create(WorkItemRequest request)
    {
      WorkItem newEntity = _repository.Add(_mapper.Map<WorkItemRequest, WorkItem>(request));
      newEntity.CreationDate = DateTime.Now;
      newEntity.CreatorId = HttpContext.GetGuidUserId();
      newEntity.Status = ValueTypes.WorkItemStatus.Created;

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
    [HttpPut("[controller]s/{workItemId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid workItemId, WorkItemRequest request)
    {
      WorkItem? entity = _repository.Get(key: workItemId, includePropertyNames: new[] { "Creator", "Executor" });
      if (entity is null)
      {
        _logger.LogInformation($"WorkItem with ID={workItemId} was not found.");
        return NotFound($"WorkItem with ID={workItemId} was not found.");
      }

      if (entity.CreatorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to WorkItem  ID={workItemId}.");
        return Forbid();
      }

      _mapper.Map<WorkItemRequest, WorkItem>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={workItemId}  was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete WorkItem by ID
    /// </summary>
    /// <param name="workItemId">WorkItem ID</param>
    [HttpDelete("[controller]s/{workItemId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid workItemId)
    {
      WorkItem? entity = _repository.Get(workItemId);
      if (entity is null)
      {
        _logger.LogInformation($"WorkItem with ID={workItemId} was not found.");
        return NotFound($"WorkItem with ID={workItemId} was not found.");
      }

      if (entity.CreatorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to WorkItem  ID={workItemId}.");
        return Forbid();
      }

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={workItemId} was deleted successfully.");

      return NoContent();

    }

    /// <summary>
    /// Change status
    /// </summary>
    /// <param name="workitemId"></param>
    /// <param name="newWorkItemStatus">New status for workItem</param>
    [HttpPut("[controller]s/{workitemId:guid}/status", Name = nameof(ChangeStatusOfWorkItem))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult ChangeStatusOfWorkItem(Guid workitemId, WorkItemStatus newWorkItemStatus)
    {
      WorkItem? workItem = _repository.Get(workitemId);
      if (workItem == null)
      {
        _logger.LogInformation($"WorkItem with ID={workitemId} was not found.");
        return NotFound($"WorkItem with ID={workitemId} was not found.");
      }

      if (newWorkItemStatus == WorkItemStatus.Canceled && workItem.CreatorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized canceled to workitem ID={workitemId}.");
        return Forbid();
      }

      workItem.Status = newWorkItemStatus;

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={workitemId} was updated successfully.");

      return NoContent();
    }
    #endregion
  }
}
