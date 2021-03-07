using AutoMapper;
using Bathhouse.Contracts;
using Bathhouse.Contracts.Models.v1.Requests;
using Bathhouse.Contracts.Models.v1.Responses;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
  public class EmployeeController : ControllerBase
  {
    protected readonly IRepository<Office, Guid> _officesRepository;
    protected readonly IRepository<WorkItem, Guid> _workItemRepository;
    protected readonly IRepository<Survey, Guid> _surveyRepository;
    protected readonly IBathhouseUnitOfWork _unitOfWork;
    protected readonly IRepository<Employee, Guid> _repository;
    protected readonly ILogger<EmployeeController> _logger;
    protected readonly IMapper _mapper;
    protected readonly UserManager<Employee> _userManager;

    public EmployeeController(
      ILogger<EmployeeController> logger,
      IMapper mapper,
      IBathhouseUnitOfWork unitOfWork,
      UserManager<Employee> userManager)
    {
      _officesRepository = unitOfWork.Repository<Office, Guid>();
      _workItemRepository = unitOfWork.Repository<WorkItem, Guid>();
      _surveyRepository = unitOfWork.Repository<Survey, Guid>();

      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Employee, Guid>();

      _userManager = userManager;
    }

    #region CRUD endpoints
    /// <summary>
    /// Get all of Employees
    /// </summary>
    [HttpGet(ApiRoutes.GetAllEmployees, Name = nameof(ApiRoutes.GetAllEmployees))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<EmployeeResponse>> GetAll([FromQuery] PaginationQuery paginationQuery)
    {
      PaginationFilter paginationFilter = new()
      {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber
      };

      var allEntities = _repository.GetAll(
        paginationFilter: paginationFilter,
        orderBy: all => all.OrderBy(employee => employee.LastName));
      _logger.LogInformation($"All of Employees was got.");

      return Ok(new PaginatedResponse<EmployeeResponse>()
      {
        Data = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(allEntities),
        PageNumber = paginationFilter.IsValid ? paginationFilter.PageNumber : null,
        PageSize = paginationFilter.IsValid ? paginationFilter.PageSize : null
      });
    }

    // TODO Add offices to response 
    /// <summary>
    /// Get Employee by ID
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet(ApiRoutes.GetEmployeeById, Name = nameof(ApiRoutes.GetEmployeeById))]
    public ActionResult<EmployeeResponse> GetById(Guid employeeId)
    {
      Employee? entity = _repository.Get(employeeId);

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"Employee id={employeeId} was getting successfully.");
      return Ok(_mapper.Map<Employee, EmployeeResponse>(entity));
    }

    /// <summary>
    /// Add Employee.
    /// </summary>
    /// <param name="request">Newly creating Employee</param>
    [Authorize(Policy = Constants.EmployeeAddOrDeletePolicy)]
    [HttpPost(ApiRoutes.CreateEmployee, Name = nameof(ApiRoutes.CreateEmployee))]
    public ActionResult<EmployeeResponse> Create(EmployeeRequest request)
    {
      Employee newEntity = _mapper.Map<EmployeeRequest, Employee>(request);
      newEntity.NormalizedEmail = newEntity.Email.ToUpper();
      newEntity.UserName = newEntity.Email;
      newEntity.NormalizedUserName = newEntity.UserName.ToUpper();

      IdentityResult result = _userManager.CreateAsync(newEntity, Constants.DefaultPassword).Result;

      if (!result.Succeeded)
      {
        _logger.LogInformation("Creating of new user is failed.");
        return BadRequest(result.Errors);
      }

      _logger.LogInformation($"Employee was creating successfully.");

      return CreatedAtAction(
        nameof(GetById),
        new { employeeId = newEntity.Id },
        _mapper.Map<Employee, EmployeeResponse>(newEntity));
    }

    /// <summary>
    /// Update Employee
    /// </summary>
    /// <param name="request">Employee for updating</param>
    /// <param name="employeeId">ID of Employee for updating</param>
    [HttpPut(ApiRoutes.UpdateEmployee, Name = nameof(ApiRoutes.UpdateEmployee))]
    public ActionResult Update(Guid employeeId, EmployeeRequest request)
    {
      if (employeeId != HttpContext.GetGuidUserId()
        && (!HttpContext.User?.IsInRole(Constants.AdminRoleName) ?? true)
        && (!HttpContext.User?.IsInRole(Constants.DirectorRoleName) ?? true)
        )
      {
        
        _logger.LogInformation($"Unauthorized acces to user id={employeeId}.");
        return Forbid();
      }

      Employee? entity = _repository.Get(employeeId);

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _mapper.Map<EmployeeRequest, Employee>(request, entity);
      IdentityResult result = _userManager.UpdateAsync(entity).Result;

      if (!result.Succeeded)
      {
        _logger.LogInformation("Creating of new user is failed.");
        return BadRequest(result.Errors);
      }

      _logger.LogInformation($"Employee id={employeeId} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete Employee by ID
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    [Authorize(Policy = Constants.EmployeeAddOrDeletePolicy)]
    [HttpDelete(ApiRoutes.DeleteEmployee, Name = nameof(ApiRoutes.DeleteEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid employeeId)
    {
      Employee? entity = _repository.Get(employeeId);
      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Employee id={employeeId} was deleted successfully.");

      return NoContent();
    }
    #endregion

    #region Roles

    /// <summary>
    /// Get roles for employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet(ApiRoutes.GetRolesForEmployee, Name = nameof(ApiRoutes.GetRolesForEmployee))]
    public ActionResult<string> GetRolesForEmployee(Guid employeeId)
    {
      Employee? entity = _repository.Get(key: employeeId);

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"Employee id={employeeId} was getting successfully.");

      return Ok(_userManager.GetRolesAsync(entity).Result);
    }

    /// <summary>
    /// Add Role to Employee.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="newRole">Name of an adding role</param>
    [Authorize(Policy = Constants.AdminRoleName)]
    [HttpPost(ApiRoutes.AddRoleForEmployee, Name = nameof(ApiRoutes.AddRoleForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IdentityResult> AddRoleForEmployee(Guid employeeId, string newRole)
    {
      Employee? entity = _repository.Get(key: employeeId);

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"Employee id={employeeId} was getting successfully.");

      var identityResult = _userManager.AddToRoleAsync(entity, newRole).Result;

      if (!identityResult.Succeeded)
      {
        _logger.LogInformation($"Error is fired while adding Employee id={employeeId} to role {newRole}.");
        return BadRequest(identityResult.Errors);
      }

      return NoContent();
    }

    /// <summary>
    /// Remove Employee from Role.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="newRole">Name of an deleting role</param>
    [Authorize(Policy = Constants.AdminRoleName)]
    [HttpDelete(ApiRoutes.DeleteRoleFromEmployee, Name = nameof(ApiRoutes.DeleteRoleFromEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteRoleFromEmployee(Guid employeeId, string newRole)
    {
      Employee? entity = _repository.Get(key: employeeId);

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"Employee id={employeeId} was getting successfully.");

      var identityResult = _userManager.RemoveFromRoleAsync(entity, newRole).Result;

      if (!identityResult.Succeeded)
      {
        _logger.LogInformation($"Error is fired while deleting Employee id={employeeId} from role {newRole}.");
        return BadRequest(identityResult.Errors);
      }

      return NoContent();
    }


    #endregion

    #region Static endpoints

    /// <summary>
    /// Get all of the directors in the system
    /// </summary>
    [HttpGet(ApiRoutes.GetAllTheDirectors, Name = nameof(ApiRoutes.GetAllTheDirectors))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllTheDirectors()
    {
      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
        _userManager.GetUsersInRoleAsync(Constants.DirectorRoleName).Result));
    }

    /// <summary>
    /// Get all of the employees in the system
    /// </summary>
    [HttpGet(ApiRoutes.GetAllSimpleEmployees, Name = nameof(ApiRoutes.GetAllSimpleEmployees))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllSimpleEmployees()
    {
      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
        _userManager.GetUsersInRoleAsync(Constants.EmployeeRoleName).Result));
    }

    /// <summary>
    /// Get all of the managers in the system
    /// </summary>
    [HttpGet(ApiRoutes.GetAllManagers, Name = nameof(ApiRoutes.GetAllManagers))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllManagers()
    {
      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
        _userManager.GetUsersInRoleAsync(Constants.ManagerRoleName).Result));
    }

    /// <summary>
    /// Get all of the tech supporters in the system
    /// </summary>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.GetAllTechSupporters, Name = nameof(ApiRoutes.GetAllTechSupporters))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllTechSupporters()
    {
      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
        _userManager.GetUsersInRoleAsync(Constants.AdminRoleName).Result));
    }

    #endregion

    #region Office
    /// <summary>
    /// Get offices for employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet(ApiRoutes.GetOfficesForEmployee, Name = nameof(ApiRoutes.GetOfficesForEmployee))]
    public ActionResult<OfficeResponse> GetOfficesForEmployee(Guid employeeId)
    {
      Employee? entity = _repository.Get(key: employeeId, includePropertyNames: new[] { "Offices" });

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"Employee id={employeeId} was getting successfully.");
      _logger.LogInformation($"Office for employee id={employeeId} was getting successfully.");

      return Ok(_mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(entity.Offices));
    }

    /// <summary>
    /// Delete office for employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="officeId">ID deleting office</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpDelete(ApiRoutes.DeleteOfficeFromEmployee, Name = nameof(ApiRoutes.DeleteOfficeFromEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult DeleteOfficeFromEmployee(Guid employeeId, Guid officeId)
    {
      Employee? entity = _repository.Get(key: employeeId, includePropertyNames: new[] { "Offices" });

      if (entity is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      entity.DeleteOffice(officeId);

      _unitOfWork.Complete();
      _logger.LogInformation($"Office of employee id={employeeId} was deleted successfully.");

      return NoContent();
    }

    /// <summary>
    /// Add office for employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="officeId">Office ID</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpPost(ApiRoutes.AddOfficeToEmployee, Name = nameof(ApiRoutes.AddOfficeToEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IEnumerable<OfficeResponse>> AddOfficeToEmployee(Guid employeeId, Guid officeId)
    {
      Employee? employee = _repository.Get(key: employeeId, includePropertyNames: new[] { "Offices" });
      if (employee is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      Office? addingOffice = _officesRepository.Get(officeId);
      if (addingOffice is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      employee.AddOffice(addingOffice);

      _unitOfWork.Complete();
      _logger.LogInformation($"Office id={officeId} was added to Employee ID={employeeId} successfully.");

      return NoContent();
    }

    /// <summary>
    /// Set new list of offices to employee
    /// </summary>
    /// <param name="employeeId">Employee  ID</param>
    /// <param name="officeIds">Office IDs</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpPut(ApiRoutes.SetOfficesForEmployee, Name = nameof(ApiRoutes.SetOfficesForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IEnumerable<OfficeResponse>> SetOfficesForEmployee(Guid employeeId, [FromBody] IEnumerable<Guid> officeIds)
    {
      Employee? employee = _repository.Get(key: employeeId, includePropertyNames: new[] { "Offices" });
      if (employee == null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      employee.Offices.Clear();

      foreach (var officeId in officeIds)
      {
        Office? addingOffice = _officesRepository.Get(officeId);

        if (addingOffice == null)
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Office with ID={officeId} was not found.");
        }

        employee.AddOffice(addingOffice);
        _logger.LogInformation($"Office id={officeId} was found.");
      }

      _unitOfWork.Complete();
      _logger.LogInformation($"Office was added to Employee ID={employeeId} successfully.");

      return NoContent();
    }

    #endregion

    #region WorkItems
    /// <summary>
    /// Get workitems for current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet(ApiRoutes.GetWorkItemsForEmployee, Name = nameof(ApiRoutes.GetWorkItemsForEmployee))]
    public ActionResult<WorkItemResponse> GetWorkItemsForEmployee(Guid employeeId)
    {
      if (_repository.Get(key: employeeId) is not Employee employee)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"WorkItems for employee id={employeeId} was getting successfully.");

      var workItems = _workItemRepository.GetAll(
        filter: wi => wi.ExecutorId == employeeId,
        includePropertyNames: new[] { "Executor", "Creator" },
        orderBy: all => all.OrderBy(wi => wi.CreationDate));
      return Ok(_mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(workItems));
    }

    /// <summary>
    /// Get workitems created by current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet(ApiRoutes.GetAllWorkItemsCreatedByEmployee, Name = nameof(ApiRoutes.GetAllWorkItemsCreatedByEmployee))]
    public ActionResult<IEnumerable<WorkItemResponse>> GetAllWorkItemsCreatedByEmployee(Guid employeeId)
    {
      if (_repository.Get(key: employeeId) is not Employee employee)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"WorkItems for employee id={employeeId} was getting successfully.");

      var workItems = _workItemRepository.GetAll(
        filter: wi => wi.CreatorId == employeeId,
        includePropertyNames: new[] { "Executor", "Creator" },
        orderBy: all => all.OrderBy(wi => wi.CreationDate));
      return Ok(_mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(workItems));
    }

    /// <summary>
    /// Get workitem created by current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    /// <param name="workitemId">WorkItem ID</param>
    [HttpGet(ApiRoutes.GetWorkItemCreatedByEmployee, Name = nameof(ApiRoutes.GetWorkItemCreatedByEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<WorkItemResponse> GetWorkItemCreatedByEmployee(Guid employeeId, Guid workitemId)
    {
      WorkItem? workItem = _workItemRepository.Get(workitemId);
      if (workItem == null)
      {
        _logger.LogInformation($"WorkItem with ID={workitemId} was not found.");
        return NotFound($"WorkItem with ID={workitemId} was not found.");
      }

      if (workItem.CreatorId != employeeId)
      {
        _logger.LogInformation($"Unauthorized access to workitem ID={workitemId}.");
        return Forbid();
      }

      return Ok(_mapper.Map<WorkItem, WorkItemResponse>(workItem));
    }

    /// <summary>
    /// Delete workItem
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="workitemId">WorkItem ID</param>
    [HttpDelete(ApiRoutes.DeleteWorkItemCreatedByEmployee, Name = nameof(ApiRoutes.DeleteWorkItemCreatedByEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteWorkItemCreatedByEmployee(Guid employeeId, Guid workitemId)
    {
      WorkItem? workItem = _workItemRepository.Get(workitemId);
      if (workItem == null)
      {
        _logger.LogInformation($"WorkItem with ID={workitemId} was not found.");
        return NotFound($"WorkItem with ID={workitemId} was not found.");
      }

      if (workItem.CreatorId != employeeId || workItem.CreatorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to workitem ID={workitemId}.");
        return Forbid();
      }

      _workItemRepository.Delete(workitemId);

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={workitemId} was deleted successfully.");

      return NoContent();
    }

    /// <summary>
    /// Create workItem.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="workItem">Newly creating workItem. WokrItemRequest.CreatorId will be overwrited by Employee ID</param>
    [HttpPost(ApiRoutes.CreateWorkItemByEmployee, Name = nameof(ApiRoutes.CreateWorkItemByEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
    public ActionResult<WorkItemResponse> CreateWorkItemByEmployee(Guid employeeId, WorkItemRequest workItem)
    {
      WorkItem newWorkItem = _workItemRepository.Add(_mapper.Map<WorkItemRequest, WorkItem>(workItem));
      newWorkItem.CreatorId = HttpContext.GetGuidUserId();
      newWorkItem.CreationDate = DateTime.Now;
      newWorkItem.Status = WorkItemStatus.Created;

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={newWorkItem.Id} was creating successfully.");

      return CreatedAtAction(
        nameof(GetWorkItemCreatedByEmployee),
        new { employeeId, workitemid = newWorkItem.Id },
        _mapper.Map<WorkItem, WorkItemResponse>(newWorkItem));
    }

    /// <summary>
    /// Update WorkItem
    /// </summary>
    /// <param name="request">WorkItem for updating</param>
    /// <param name="employeeId">ID of entity for updating</param>
    /// <param name="workitemId"></param>
    [HttpPut(ApiRoutes.UpdateCreatedWorkItem, Name = nameof(ApiRoutes.UpdateCreatedWorkItem))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult UpdateCreatedWorkItem(Guid employeeId, Guid workitemId, WorkItemRequest request)
    {
      WorkItem? workItem = _workItemRepository.Get(workitemId);
      if (workItem == null)
      {
        _logger.LogInformation($"WorkItem with ID={workitemId} was not found.");
        return NotFound($"WorkItem with ID={workitemId} was not found.");
      }

      if (workItem.CreatorId != employeeId || workItem.CreatorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to workitem ID={workitemId}.");
        return Forbid();
      }

      WorkItem updatedEntity = _mapper.Map<WorkItemRequest, WorkItem>(request, workItem);

      _unitOfWork.Complete();
      _logger.LogInformation($"WorkItem id={updatedEntity.Id} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Change status for one of the MyWorkItem
    /// </summary>
    /// <param name="employeeId">ID of entity for updating</param>
    /// <param name="workitemId"></param>
    /// <param name="newWorkItemStatus">New status for workItem</param>
    [HttpPut(ApiRoutes.ChangeStatusMyWorkItem, Name = nameof(ApiRoutes.ChangeStatusMyWorkItem))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult ChangeStatusMyWorkItem(Guid employeeId, Guid workitemId, WorkItemStatus newWorkItemStatus)
    {
      WorkItem? workItem = _workItemRepository.Get(workitemId);
      if (workItem == null)
      {
        _logger.LogInformation($"WorkItem with ID={workitemId} was not found.");
        return NotFound($"WorkItem with ID={workitemId} was not found.");
      }

      if (newWorkItemStatus == WorkItemStatus.Canceled 
        && (workItem.CreatorId != employeeId ||
            workItem.CreatorId != HttpContext.GetGuidUserId()))
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

    #region Survey

    /// <summary>
    /// Get all of surveys for current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet(ApiRoutes.GetAllSurveysForEmployee, Name = nameof(ApiRoutes.GetAllSurveysForEmployee))]
    public ActionResult<IEnumerable<SurveyResponse>> GetAllSurveysForEmployee(Guid employeeId)
    {
      Employee? employee = _repository.Get(key: employeeId, includePropertyNames: new[] { "Surveys" });
      if (employee == null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      _logger.LogInformation($"Employee id={employeeId} was getting successfully.");
      _logger.LogInformation($"Surveys for employee id={employeeId} was getting successfully.");

      return Ok(_mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyResponse>>(employee.Surveys));
    }

    /// <summary>
    /// Get survey summary
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    /// <param name="summarytype">Summary type</param>
    [HttpGet(ApiRoutes.GetSurveySummaryForEmployee, Name = nameof(ApiRoutes.GetSurveySummaryForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<SurveySummaryResponse> GetSurveySummaryForEmployee(Guid employeeId, Guid surveyId, SurveyResultSummaryType summarytype)
    {
      Survey? survey = _surveyRepository.Get(
        key: surveyId,
        includePropertyNames: new[] { "Questions", "Author" });
      if (survey == null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (survey.AuthorId != employeeId || survey.AuthorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to survey ID={surveyId}.");
        return Forbid();
      }

      survey.Results = _unitOfWork.SurveyResults
        .GetAll(
            filter: result => result.SurveyId == survey.Id,
            includePropertyNames: new[] { "Author", "Answers" })
        .ToList();

      var summary = survey.GetSummary(summarytype);
      return Ok(_mapper.Map<SurveySummary, SurveySummaryResponse>(summary));
    }

    /// <summary>
    /// Delete Survey
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    [HttpDelete(ApiRoutes.DeleteSurveyForEmployee, Name = nameof(ApiRoutes.DeleteSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteSurveyForEmployee(Guid employeeId, Guid surveyId)
    {
      Survey? survey = _surveyRepository.Get(surveyId);
      if (survey == null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (survey.AuthorId != employeeId || survey.AuthorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to survey ID={surveyId}.");
        return Forbid();
      }

      _surveyRepository.Delete(surveyId);

      _unitOfWork.Complete();
      _logger.LogInformation($"Survey id={surveyId} was deleted successfully.");

      return NoContent();
    }

    /// <summary>
    /// Get survey
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    [HttpGet(ApiRoutes.GetSurveyForEmployee, Name = nameof(ApiRoutes.GetSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<SurveyResponse> GetSurveyForEmployee(Guid employeeId, Guid surveyId)
    {
      Survey? survey = _surveyRepository.Get(key: surveyId, includePropertyNames: new[] { "Questions" });
      if (survey == null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} created by {employeeId} was not found.");
        return NotFound($"Survey with ID={surveyId} created by {employeeId} was not found.");
      }

      if (survey.AuthorId != employeeId)
      {
        _logger.LogInformation($"Unauthorized acces to survey with ID={surveyId} created by {employeeId}.");
        return Forbid();
      }

      return Ok(_mapper.Map<Survey, SurveyResponse>(survey));
    }

    /// <summary>
    /// Create Survey.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="survey">Newly creating Survey. SurveyRequest.AuthorId will be overwrited by Employee ID</param>
    [HttpPost(ApiRoutes.CreateSurveyForEmployee, Name = nameof(ApiRoutes.CreateSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
    public ActionResult<SurveyResponse> CreateSurveyForEmployee(Guid employeeId, SurveyRequest survey)
    {
      Survey newSurvey = _surveyRepository.Add(_mapper.Map<SurveyRequest, Survey>(survey));
      newSurvey.AuthorId = HttpContext.GetGuidUserId();
      newSurvey.CreationDate = DateTime.Now;
      newSurvey.Status = SurveyStatus.Work;

      _unitOfWork.Complete();
      _logger.LogInformation($"Survey id={newSurvey.Id} was creating successfully.");

      return CreatedAtAction(
        nameof(GetSurveyForEmployee), 
        new { employeeId, surveyId = newSurvey.Id }, 
        _mapper.Map<Survey, SurveyResponse>(newSurvey));
    }

    /// <summary>
    /// Update Survey
    /// </summary>
    /// <param name="employeeId">ID of entity for updating</param>
    /// <param name="surveyId">Survey for updating</param>
    /// <param name="newName">New name</param>
    /// <param name="newDescription">New Description</param>
    [HttpPut(ApiRoutes.UpdateSurveyForEmployee, Name = nameof(ApiRoutes.UpdateSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult UpdateSurveyForEmployee(
      Guid employeeId, 
      Guid surveyId, 
      string? newName, 
      string? newDescription)
    {
      Survey? survey = _surveyRepository.Get(surveyId);
      if (survey == null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (survey.AuthorId != employeeId || survey.AuthorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to survey ID={surveyId}.");
        return Forbid();
      }

      survey.Name = newName ?? survey.Name;
      survey.Description = newDescription ?? survey.Description;

      _unitOfWork.Complete();
      _logger.LogInformation($"Survey id={survey.Id} was updated successfully.");

      return NoContent();
    }
    #endregion
  }
}
