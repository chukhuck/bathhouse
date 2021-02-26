using AutoMapper;
using Bathhouse.Contracts;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace Bathhouse.Api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
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
    [HttpGet(Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<IEnumerable<EmployeeResponse>> GetAll()
    {
      try
      {
        var allEntities = _repository.GetAll();
        _logger.LogInformation($"All of Employees was got.");

        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(allEntities));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of Employees an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of Employees an exception was fired.");
      }
    }

    // TODO Add offices to response 
    /// <summary>
    /// Get Employee by ID
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet("{employeeId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<EmployeeResponse> GetById(Guid employeeId)
    {
      try
      {
        Employee? entity = _repository.Get(employeeId);

        if (entity is null)
        {
          _logger.LogInformation($"Request on getting unexisting Employee id={employeeId} was received.");
          return NotFound($"Employee with ID={employeeId} was not found.");
        }

        _logger.LogInformation($"Employee id={employeeId} was getting successfully.");
        return Ok(_mapper.Map<Employee, EmployeeResponse>(entity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Employee id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Add Employee.
    /// </summary>
    /// <param name="request">Newly creating Employee</param>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<EmployeeResponse> Create(EmployeeRequest request)
    {
      try
      {
        Employee newEntity = _repository.Add(_mapper.Map<EmployeeRequest, Employee>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"Employee id= was creating successfully.");

        return CreatedAtAction(
          "GetById", 
          new { id = newEntity.Id }, 
          _mapper.Map<Employee, EmployeeResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating Employee an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating Employee an exception was fired");
      }
    }

    /// <summary>
    /// Update Employee
    /// </summary>
    /// <param name="request">Employee for updating</param>
    /// <param name="employeeId">ID of Employee for updating</param>
    [HttpPut("{employeeId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid employeeId, EmployeeRequest request)
    {
      try
      {
        Employee? entity = _repository.Get(employeeId);

        if (entity is null)
        {
          _logger.LogInformation($"Employee with ID={employeeId} was not found.");
          return NotFound($"Employee with ID={employeeId} was not found.");
        }

        Employee updatedEntity = _mapper.Map<EmployeeRequest, Employee>(request, entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Employee id={employeeId} was updated successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Employee an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Employee an exception was fired");
      }
    }

    /// <summary>
    /// Delete Employee by ID
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    [HttpDelete("{employeeId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid employeeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Employee id={employeeId} an exception was fired");
      }
    }
    #endregion

    #region Roles

    /// <summary>
    /// Get roles for employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet("{employeeId:guid}/roles", Name = nameof(GetRolesForEmployee))]
    public ActionResult<string> GetRolesForEmployee(Guid employeeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting roles of employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting roles of employee id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Add Role to Employee.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="newRole">Name of an adding role</param>
    [HttpPost("{employeeId:guid}/roles", Name = nameof(AddRoleForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IdentityResult> AddRoleForEmployee(Guid employeeId, string newRole)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While adding Employee id={employeeId} to role {newRole} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While adding Employee id={employeeId} to role {newRole} an exception was fired");
      }
    }

    /// <summary>
    /// Remove Employee from Role.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="newRole">Name of an deleting role</param>
    [HttpDelete("{employeeId:guid}/roles", Name = nameof(DeleteRoleFromEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteRoleFromEmployee(Guid employeeId, string newRole)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Employee id={employeeId} from role {newRole} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Employee id={employeeId} from role {newRole} an exception was fired");
      }
    }


    #endregion

    #region Static endpoints

    /// <summary>
    /// Get all of the directors in the system
    /// </summary>
    [HttpGet("directors", Name = nameof(GetAllTheDirectors))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllTheDirectors()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
          _userManager.GetUsersInRoleAsync(Constants.DirectorRoleName).Result));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting directors an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting directors an exception was fired");
      }
    }

    /// <summary>
    /// Get all of the employees in the system
    /// </summary>
    [HttpGet("employees", Name = nameof(GetAllSimpleEmployees))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllSimpleEmployees()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
          _userManager.GetUsersInRoleAsync(Constants.EmployeeRoleName).Result));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting employees an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting employees an exception was fired");
      }
    }

    /// <summary>
    /// Get all of the managers in the system
    /// </summary>
    [HttpGet("managers", Name = nameof(GetAllManagers))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllManagers()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
          _userManager.GetUsersInRoleAsync(Constants.ManagerRoleName).Result));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting managers an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting managers an exception was fired");
      }
    }

    /// <summary>
    /// Get all of the tech supporters in the system
    /// </summary>
    [HttpGet("techsupporters", Name = nameof(GetAllTechSupporters))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<EmployeeResponse> GetAllTechSupporters()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
          _userManager.GetUsersInRoleAsync(Constants.AdminRoleName).Result));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting the tech supporters an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While the tech supporters directors an exception was fired");
      }
    }

    #endregion

    #region Office
    /// <summary>
    /// Get offices for employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet("{employeeId:guid}/offices", Name = nameof(GetOfficesForEmployee))]
    public ActionResult<OfficeResponse> GetOfficesForEmployee(Guid employeeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting offices of employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting offices of employee id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Delete office for employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="officeId">ID deleting office</param>
    [HttpDelete("{employeeId:guid}/offices/{officeId:guid}", Name = nameof(DeleteOfficeFromEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult DeleteOfficeFromEmployee(Guid employeeId, Guid officeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While deleting office for manager  id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting office for manager  id={employeeId}  an exception was fired");
      }
    }

    /// <summary>
    /// Add office for employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="officeId">Office ID</param>
    [HttpPost("{employeeId:guid}/offices", Name = nameof(AddOfficeToEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IEnumerable<OfficeResponse>> AddOfficeToEmployee(Guid employeeId, Guid officeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While adding office to employee an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While adding office to employee  an exception was fired");
      }
    }

    /// <summary>
    /// Set new list of offices to employee
    /// </summary>
    /// <param name="employeeId">Employee  ID</param>
    /// <param name="officeIds">Office IDs</param>
    [HttpPut("{employeeId:guid}/offices", Name = nameof(SetOfficesForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IEnumerable<OfficeResponse>> SetOfficesForEmployee(Guid employeeId, [FromBody] IEnumerable<Guid> officeIds)
    {
      try
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

          employee.Offices.Add(addingOffice);
          _logger.LogInformation($"Office id={officeId} was found.");
        }

        _unitOfWork.Complete();
        _logger.LogInformation($"Office was added to Employee ID={employeeId} successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While setting offices to employee an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While setting offices to employee an exception was fired");
      }
    }

    #endregion

    #region WorkItems
    /// <summary>
    /// Get workitems for current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet("{employeeId:guid}/myworkitems", Name = nameof(GetWorkItemsForEmployee))]
    public ActionResult<WorkItemResponse> GetWorkItemsForEmployee(Guid employeeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting workitems of employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting workitems of employee id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Get workitems created by current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet("{employeeId:guid}/workitems", Name = nameof(GetAllWorkItemsCreatedByEmployee))]
    public ActionResult<IEnumerable<WorkItemResponse>> GetAllWorkItemsCreatedByEmployee(Guid employeeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting workitems created by employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting workitems created by employee id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Get workitem created by current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    /// <param name="workitemId">WorkItem ID</param>
    [HttpGet("{employeeId:guid}/workitems/{workitemId:guid}", Name = nameof(GetWorkItemCreatedByEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<WorkItemResponse> GetWorkItemCreatedByEmployee(Guid employeeId, Guid workitemId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting workitem id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting workitem id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Delete workItem
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="workitemId">WorkItem ID</param>
    [HttpDelete("{employeeId:guid}/workitems/{workitemId:guid}", Name = nameof(DeleteWorkItemCreatedByEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteWorkItemCreatedByEmployee(Guid employeeId, Guid workitemId)
    {
      try
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

        _workItemRepository.Delete(workitemId);

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={workitemId} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting WorkItem id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting WorkItem id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Create workItem.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="workItem">Newly creating workItem. WokrItemRequest.CreatorId will be overwrited by Employee ID</param>
    [HttpPost("{employeeId:guid}/workitems", Name = nameof(CreateWorkItemByEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
    public ActionResult<WorkItemResponse> CreateWorkItemByEmployee(Guid employeeId, WorkItemRequest workItem)
    {
      try
      {
        workItem.CreatorId = employeeId;
        WorkItem newWorkItem = _workItemRepository.Add(_mapper.Map<WorkItemRequest, WorkItem>(workItem));

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={newWorkItem.Id} was creating successfully.");

        return CreatedAtAction(
          "GetCreatedWorkItem", 
          new { employeeId, workitemid = newWorkItem.Id }, 
          _mapper.Map<WorkItem, WorkItemResponse>(newWorkItem));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating workitem an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating workitem an exception was fired");
      }
    }

    /// <summary>
    /// Update WorkItem
    /// </summary>
    /// <param name="request">WorkItem for updating</param>
    /// <param name="employeeId">ID of entity for updating</param>
    /// <param name="workitemId"></param>
    [HttpPut("{employeeId:guid}/workitems/{workitemId:guid}", Name = nameof(UpdateCreatedWorkItem))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult UpdateCreatedWorkItem(Guid employeeId, Guid workitemId, WorkItemRequest request)
    {
      try
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

        request.CreatorId = employeeId;
        WorkItem updatedEntity = _mapper.Map<WorkItemRequest, WorkItem>(request, workItem);

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={updatedEntity.Id} was updated successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating workitem an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating workitem an exception was fired");
      }
    }

    /// <summary>
    /// Change status for one of the MyWorkItem
    /// </summary>
    /// <param name="employeeId">ID of entity for updating</param>
    /// <param name="workitemId"></param>
    /// <param name="newWorkItemStatus">New status for workItem</param>
    [HttpPut("{employeeId:guid}/workitems/{workitemId:guid}/status", Name = nameof(ChangeStatusMyWorkItem))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult ChangeStatusMyWorkItem(Guid employeeId, Guid workitemId, WorkItemStatus newWorkItemStatus)
    {
      try
      {
        WorkItem? workItem = _workItemRepository.Get(workitemId);
        if (workItem == null)
        {
          _logger.LogInformation($"WorkItem with ID={workitemId} was not found.");
          return NotFound($"WorkItem with ID={workitemId} was not found.");
        }

        if (workItem.ExecutorId == employeeId)
        {
          _logger.LogInformation($"Unauthorized access to workitem ID={workitemId}.");
          return Forbid();
        }

        if (newWorkItemStatus == WorkItemStatus.Canceled && workItem.CreatorId != employeeId)
        {
          _logger.LogInformation($"Unauthorized canceled to workitem ID={workitemId}.");
          return Forbid();
        }

        workItem.Status = newWorkItemStatus;

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={employeeId} was updated successfully.");

        return NoContent();

      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating workitem an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating workitem an exception was fired");
      }
    }

    #endregion

    #region Survey

    /// <summary>
    /// Get all of surveys for current employee
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    [HttpGet("{employeeId:guid}/surveys", Name = nameof(GetAllSurveysForEmployee))]
    public ActionResult<IEnumerable<SurveyResponse>> GetAllSurveysForEmployee(Guid employeeId)
    {
      try
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting surveys of employee id={employeeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting surveys of employee id={employeeId} an exception was fired");
      }
    }

    /// <summary>
    /// Get survey
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    [HttpGet("{employeeId:guid}/surveys/{surveyId:guid}", Name = nameof(GetSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<SurveyResponse> GetSurveyForEmployee(Guid employeeId, Guid surveyId)
    {
      try
      {
        Survey? survey = _surveyRepository.Get(key: surveyId, includePropertyNames: new[] { "Questions" });
        if (survey == null)
        {
          _logger.LogInformation($"Survey with ID={surveyId} was not found.");
          return NotFound($"Survey with ID={surveyId} was not found.");
        }

        if (survey.AuthorId != employeeId)
        {
          _logger.LogInformation($"Unauthorized access to survey ID={surveyId}.");
          return Forbid();
        }

        return Ok(_mapper.Map<Survey, SurveyResponse>(survey));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Survey id={surveyId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Survey id={surveyId} an exception was fired");
      }
    }

    /// <summary>
    /// Get survey summary
    /// </summary>
    /// <param name="employeeId">The Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    /// <param name="summarytype">Summary type</param>
    [HttpGet("{employeeId:guid}/surveys/{surveyId:guid}/summary", Name = nameof(GetSurveySummaryForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<SurveySummaryResponse> GetSurveySummaryForEmployee(Guid employeeId, Guid surveyId, SurveyResultSummaryType summarytype)
    {
      try
      {
        Survey? survey = _surveyRepository.Get(
          key: surveyId,
          includePropertyNames: new[] { "Questions", "Author" });
        if (survey == null)
        {
          _logger.LogInformation($"Survey with ID={surveyId} was not found.");
          return NotFound($"Survey with ID={surveyId} was not found.");
        }

        if (survey.AuthorId != employeeId)
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
      catch (Exception ex)
      {
        _logger.LogError($"While getting Survey id={surveyId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Survey id={surveyId} an exception was fired");
      }
    }

    /// <summary>
    /// Delete Survey
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    [HttpDelete("{employeeId:guid}/surveys/{surveyId:guid}", Name = nameof(DeleteSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteSurveyForEmployee(Guid employeeId, Guid surveyId)
    {
      try
      {
        Survey? survey = _surveyRepository.Get(surveyId);
        if (survey == null)
        {
          _logger.LogInformation($"Survey with ID={surveyId} was not found.");
          return NotFound($"Survey with ID={surveyId} was not found.");
        }

        if (survey.AuthorId != employeeId)
        {
          _logger.LogInformation($"Unauthorized access to survey ID={surveyId}.");
          return Forbid();
        }

        _surveyRepository.Delete(surveyId);

        _unitOfWork.Complete();
        _logger.LogInformation($"Survey id={surveyId} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Survey id={surveyId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Survey id={surveyId} an exception was fired");
      }
    }

    /// <summary>
    /// Create Survey.
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="survey">Newly creating Survey. SurveyRequest.AuthorId will be overwrited by Employee ID</param>
    [HttpPost("{employeeId:guid}/surveys", Name = nameof(CreateSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
    public ActionResult<SurveyResponse> CreateSurveyForEmployee(Guid employeeId, SurveyRequest survey)
    {
      try
      {
        survey.AuthorId = employeeId;
        Survey newSurvey = _surveyRepository.Add(_mapper.Map<SurveyRequest, Survey>(survey));

        _unitOfWork.Complete();
        _logger.LogInformation($"Survey id={newSurvey.Id} was creating successfully.");

        return CreatedAtAction("GetSurvey", new { employeeId, surveyId = newSurvey.Id }, _mapper.Map<Survey, SurveyResponse>(newSurvey));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating Survey an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating Survey an exception was fired");
      }
    }

    /// <summary>
    /// Update Survey
    /// </summary>
    /// <param name="request">Survey for updating</param>
    /// <param name="employeeId">ID of entity for updating</param>
    /// <param name="surveyId"></param>
    [HttpPut("{employeeId:guid}/surveys/{surveyId:guid}", Name = nameof(UpdateSurveyForEmployee))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult UpdateSurveyForEmployee(Guid employeeId, Guid surveyId, SurveyRequest request)
    {
      try
      {
        Survey? survey = _surveyRepository.Get(surveyId);
        if (survey == null)
        {
          _logger.LogInformation($"Survey with ID={surveyId} was not found.");
          return NotFound($"Survey with ID={surveyId} was not found.");
        }

        if (survey.AuthorId != employeeId)
        {
          _logger.LogInformation($"Unauthorized access to survey ID={surveyId}.");
          return Forbid();
        }

        request.AuthorId = employeeId;
        Survey updatedEntity = _mapper.Map<SurveyRequest, Survey>(request, survey);

        _unitOfWork.Complete();
        _logger.LogInformation($"Survey id={updatedEntity.Id} was updated successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Survey an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Survey an exception was fired");
      }
    }
    #endregion
  }
}
