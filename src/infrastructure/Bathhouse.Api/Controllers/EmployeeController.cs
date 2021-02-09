using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Bathhouse.ValueTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EmployeeController : ControllerBase
  {
    protected readonly IRepository<Office, Guid> _officesRepository;
    protected readonly IRepository<WorkItem, Guid> _workItemRepository;
    protected readonly IRepository<Survey, Guid> _surveyRepository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IRepository<Employee, Guid> _repository;
    protected readonly ILogger<EmployeeController> _logger;
    protected readonly IMapper _mapper;

    public EmployeeController(
      ILogger<EmployeeController> logger,
      IMapper mapper,
      IUnitOfWork unitOfWork)
    {
      _officesRepository = unitOfWork.Repository<Office, Guid>();
      _workItemRepository = unitOfWork.Repository<WorkItem, Guid>();
      _surveyRepository = unitOfWork.Repository<Survey, Guid>();

      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Employee, Guid>();
    }

    #region CRUD endpoints
    /// <summary>
    /// Get all of Employees
    /// </summary>
    /// <response code="200">Getting all of Employees was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<IEnumerable<EmployeeResponse>> Get()
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
    /// <param name="id">The Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="200">Getting Employee is successul</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<EmployeeResponse> GetById(Guid id)
    {
      try
      {
        if (_repository.Get(id) is Employee entity)
        {
          _logger.LogInformation($"Employee id={id} was getting successfully.");
          return Ok(_mapper.Map<Employee, EmployeeResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Employee id={id} was received.");
          return NotFound($"Employee with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Employee id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Employee id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Add Employee.
    /// </summary>
    /// <param name="request">Newly creating Employee</param>
    /// <response code="201">Creating Employee is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<EmployeeResponse> Create(EmployeeRequest request)
    {
      try
      {
        Employee newEntity = _repository.Add(_mapper.Map<EmployeeRequest, Employee>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"Employee id= was creating successfully.");

        return CreatedAtAction("GetById", new { id = newEntity.Id }, _mapper.Map<Employee, EmployeeResponse>(newEntity));
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
    /// <param name="id">ID of Employee for updating</param>
    /// <response code="204">Updating Employee is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Employee with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult Update(Guid id, EmployeeRequest request)
    {
      try
      {
        if (_repository.Get(id) is Employee entity)
        {
          Employee updatedEntity = _mapper.Map<EmployeeRequest, Employee>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"Employee id={id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }
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
    /// <param name="id">Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="204">Deleting Employee is successul</response>
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
        Employee? entity = _repository.Get(id);
        if (entity is null)
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Employee id={id} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Employee id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Employee id={id} an exception was fired");
      }
    }
    #endregion



    #region Static endpoints

    //TODO Add rolemanager
    /// <summary>
    /// Get all of the directors in the system
    /// </summary>
    /// <response code="200">Getting the directors is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("directors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EmployeeResponse> GetDirectors()
    {
      try
      {
        return Ok();// _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.Director)));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting directors an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting directors an exception was fired");
      }
    }

    //TODO Add rolemanager
    /// <summary>
    /// Get all of the employees in the system
    /// </summary>
    /// <response code="200">Getting employees is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("employees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EmployeeResponse> GetEmployees()
    {
      try
      {
        return Ok();// _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.Employee)));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting employees an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting employees an exception was fired");
      }
    }

    //TODO Add rolemanager
    /// <summary>
    /// Get all of the managers in the system
    /// </summary>
    /// <response code="200">Getting the managers is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("managers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EmployeeResponse> GetManagers()
    {
      try
      {
        return Ok();// _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.Manager)));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting managers an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting managers an exception was fired");
      }
    }

    //TODO Add rolemanager
    /// <summary>
    /// Get all of the tech supporters in the system
    /// </summary>
    /// <response code="200">Getting the tech supporters is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("techsupporters")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EmployeeResponse> GetTechSupporters()
    {
      try
      {
        return Ok();//_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.TechnicalSupport)));
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
    /// <param name="id">The Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="200">Getting offices is successul.</response>
    /// <response code="400">If the ID is not valid</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/offices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<OfficeResponse> GetOffices(Guid id)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Offices" }) is Employee employee)
        {
          _logger.LogInformation($"Employee id={id} was getting successfully.");
          _logger.LogInformation($"Office for employee id={id} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(employee.Offices));
        }
        else
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting offices of employee id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting offices of employee id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Delete office for employee
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="officeId">ID deleting office</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="204">Deleting office is successul</response>
    /// <response code="400">If the request is not valid</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [Route("{id:guid}/offices/{officeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual IActionResult DeleteOffice(Guid id, Guid officeId)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Offices" }) is Employee employee)
        {
          employee.DeleteOffice(officeId);

          _unitOfWork.Complete();
          _logger.LogInformation($"Office of employee id={id} was deleted successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting office for manager  id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting office for manager  id={id}  an exception was fired");
      }
    }

    /// <summary>
    /// Add office for employee
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="officeId">Office ID</param>
    /// <response code="201">Adding office is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Employee or Office was not found</response>
    [HttpPost]
    [Route("{id:guid}/offices/{officeId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult<IEnumerable<OfficeResponse>> AddOffice(Guid id, Guid officeId)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Offices" }) is Employee employee && _officesRepository.Get(officeId) is Office addingOffice)
        {
          employee.AddOffice(addingOffice);

          _unitOfWork.Complete();
          _logger.LogInformation($"Office id={officeId} was added to Employee ID={id} successfully.");

          return CreatedAtAction(nameof(GetOffices), new { id }, _mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(employee.Offices));
        }
        else
        {
          _logger.LogInformation($"Employee with ID={id} or Office with ID={officeId} was not found.");
          return NotFound($"Employee with ID={id} or Office with ID={officeId} was not found.");
        }
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
    /// <param name="id">Employee  ID</param>
    /// <param name="officeIds">Office IDs</param>
    /// <response code="201">Setting office is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Office with current ID or one of Employee IDs is not found</response>
    [HttpPut]
    [Route("{id:guid}/offices")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult<IEnumerable<OfficeResponse>> SetEmployees(Guid id, [FromBody] IEnumerable<Guid> officeIds)
    {
      try
      {
        Employee? employee = _repository.Get(key: id, includePropertyNames: new[] { "Offices" });
        if (employee == null)
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
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
        _logger.LogInformation($"Office was added to Employee ID={id} successfully.");

        return CreatedAtAction(nameof(GetOffices), new { id }, _mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(employee.Offices));
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
    /// <param name="id">The Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="200">Getting offices is successul.</response>
    /// <response code="400">If the ID is not valid</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/myworkitems")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<WorkItemResponse> GetMyWorkItems(Guid id)
    {
      try
      {
        if (_repository.Get(key: id) is not Employee employee)
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }

        _logger.LogInformation($"WorkItems for employee id={id} was getting successfully.");

        var workItems = _workItemRepository.GetAll(
          filter: wi => wi.ExecutorId == id,
          includePropertyNames: new[] { "Executor", "Creator" },
          orderBy: all => all.OrderBy(wi => wi.CreationDate));
        return Ok(_mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(workItems));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting workitems of employee id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting workitems of employee id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Get workitems created by current employee
    /// </summary>
    /// <param name="id">The Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="200">Getting offices is successul.</response>
    /// <response code="400">If the ID is not valid</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/workitems")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<WorkItemResponse>> GetAllCreatedWorkItems(Guid id)
    {
      try
      {
        if (_repository.Get(key: id) is not Employee employee)
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }

        _logger.LogInformation($"WorkItems for employee id={id} was getting successfully.");

        var workItems = _workItemRepository.GetAll(
          filter: wi => wi.CreatorId == id,
          includePropertyNames: new[] { "Executor", "Creator" },
          orderBy: all => all.OrderBy(wi => wi.CreationDate));
        return Ok(_mapper.Map<IEnumerable<WorkItem>, IEnumerable<WorkItemResponse>>(workItems));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting workitems created by employee id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting workitems created by employee id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Get workitem created by current employee
    /// </summary>
    /// <param name="id">The Employee ID</param>
    /// <param name="workitemId">WorkItem ID</param>
    /// <response code="404">Employee or WorkItem is not found</response>
    /// <response code="200">Getting offices is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/workitems/{workitemId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<WorkItemResponse> GetCreatedWorkItem(Guid id, Guid workitemId)
    {
      try
      {
        var workItem = _workItemRepository.Get(workitemId);

        if (workItem?.CreatorId != id)
        {
          _logger.LogInformation($"Employee with ID={id} or WorkItem with ID={id} was not found.");
          return NotFound($"Employee with ID={id} or WorkItem with ID={id} was not found.");
        }

        return Ok(_mapper.Map<WorkItem, WorkItemResponse>(workItem));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting workitem id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting workitem id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Delete workItem
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="workitemId">WorkItem ID</param>
    /// <response code="404">Employee or WorkItem is not found</response>
    /// <response code="204">Deleting workItem is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete()]
    [Route("{id:guid}/workitems/{workitemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteCreatedWorkItem(Guid id, Guid workitemId)
    {
      try
      {
        WorkItem? workItem = _workItemRepository.Get(workitemId); ;

        if (workItem?.CreatorId != id)
        {
          _logger.LogInformation($"Employee with ID={id} or WorkItem with ID={id} was not found.");
          return NotFound($"Employee with ID={id} or WorkItem with ID={id} was not found.");
        }

        _workItemRepository.Delete(workitemId);

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={workitemId} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting WorkItem id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting WorkItem id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Create workItem.
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="workItem">Newly creating workItem. WokrItemRequest.CreatorId will be overwrited by Employee ID</param>
    /// <response code="201">Creating workItem is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [Route("{id:guid}/workitems")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult<WorkItemResponse> CreateWorkItem(Guid id, WorkItemRequest workItem)
    {
      try
      {
        workItem.CreatorId = id;
        WorkItem newWorkItem = _workItemRepository.Add(_mapper.Map<WorkItemRequest, WorkItem>(workItem));

        _unitOfWork.Complete();
        _logger.LogInformation($"WorkItem id={newWorkItem.Id} was creating successfully.");

        return CreatedAtAction("GetCreatedWorkItem", new { id, workitemid = newWorkItem.Id }, _mapper.Map<WorkItem, WorkItemResponse>(newWorkItem));
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
    /// <param name="id">ID of entity for updating</param>
    /// <param name="workitemId"></param>
    /// <response code="204">Updating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Entity with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}/workitems/{workitemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult UpdateCreatedWorkItem(Guid id, Guid workitemId, WorkItemRequest request)
    {
      try
      {
        if (_workItemRepository.Get(workitemId) is WorkItem workItem && workItem.CreatorId == id)
        {
          request.CreatorId = id;
          WorkItem updatedEntity = _mapper.Map<WorkItemRequest, WorkItem>(request, workItem);

          _unitOfWork.Complete();
          _logger.LogInformation($"WorkItem id={updatedEntity.Id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"WorkItem with ID={id} of type {typeof(WorkItem)} was not found.");
          return NotFound($"WorkItem with ID={id} of type {typeof(WorkItem)} was not found.");
        }
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
    /// <param name="id">ID of entity for updating</param>
    /// <param name="workItemId"></param>
    /// <param name="newWorkItemStatus">New status for workItem</param>
    /// <response code="204">Updating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If you try to cancel workitem</response>
    /// <response code="404">Entity with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}/myworkitems/{workItemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult ChangeStatusMyWorkItem(Guid id, Guid workItemId, WorkItemStatus newWorkItemStatus)
    {
      try
      {
        if (_workItemRepository.Get(workItemId) is WorkItem workItem && workItem.ExecutorId == id)
        {
          if (newWorkItemStatus == WorkItemStatus.Canceled && workItem.CreatorId != id)
          {
            _logger.LogInformation($"Employee {id} tryied to cancel WorkItem id={workItemId}. The operation is denied.");
            return BadRequest("Employee cant cancel workitem for you if he is not a creator.");
          }

          workItem.Status = newWorkItemStatus;

          _unitOfWork.Complete();
          _logger.LogInformation($"WorkItem id={id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"WorkItem with ID={id} of type {typeof(WorkItem)} was not found.");
          return NotFound($"WorkItem with ID={id} of type {typeof(WorkItem)} was not found.");
        }
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
    /// <param name="id">The Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="200">Getting offices is successul.</response>
    /// <response code="400">If the ID is not valid</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/surveys")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<SurveyResponse>> GetAllSurveys(Guid id)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Surveys" }) is Employee employee)
        {
          _logger.LogInformation($"Employee id={id} was getting successfully.");
          _logger.LogInformation($"Surveys for employee id={id} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyResponse>>(employee.Surveys));
        }
        else
        {
          _logger.LogInformation($"Employee with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting surveys of employee id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting surveys of employee id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Get survey
    /// </summary>
    /// <param name="id">The Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    /// <response code="404">Employee or Survey is not found</response>
    /// <response code="200">Getting Survey is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/surveys/{surveyId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SurveyResponse> GetSurvey(Guid id, Guid surveyId)
    {
      try
      {
        var survey = _surveyRepository.Get(key: surveyId, includePropertyNames: new[] { "Questions" });

        if (survey?.AuthorId != id)
        {
          _logger.LogInformation($"Employee with ID={id} or Survey with ID={surveyId} was not found.");
          return NotFound($"Employee with ID={id} or Survey with ID={surveyId} was not found.");
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
    /// <param name="id">The Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    /// <param name="summarytype">Summary type</param>
    /// <response code="404">Employee or Survey is not found</response>
    /// <response code="200">Getting Survey summary is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/surveys/{surveyId:guid}/summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SurveySummaryResponse> GetSurveyResult(Guid id, Guid surveyId, SurveyResultSummaryType summarytype)
    {
      try
      {
        var survey = _surveyRepository.Get(
          key: surveyId,
          includePropertyNames: new[] { "Questions", "Author" });

        Guid realSurveyId = survey?.Id ?? Guid.Empty;

        if (survey is not null)
        {
          survey.Results = _unitOfWork.SurveyResults
            .GetAll(
                filter: result => result.SurveyId == realSurveyId,
                includePropertyNames: new[] { "Author", "Answers" })
            .ToList();
        }

        if (survey?.AuthorId != id)
        {
          _logger.LogInformation($"Employee with ID={id} or Survey with ID={surveyId} was not found.");
          return NotFound($"Employee with ID={id} or Survey with ID={surveyId} was not found.");
        }
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
    /// <param name="id">Employee ID</param>
    /// <param name="surveyId">Survey ID</param>
    /// <response code="404">Employee or Survey is not found</response>
    /// <response code="204">Deleting Survey is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete()]
    [Route("{id:guid}/surveys/{surveyId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteSurvey(Guid id, Guid surveyId)
    {
      try
      {
        Survey? survey = _surveyRepository.Get(surveyId); ;

        if (survey?.AuthorId != id)
        {
          _logger.LogInformation($"Employee with ID={id} or Survey with ID={surveyId} was not found.");
          return NotFound($"Employee with ID={id} or Survey with ID={surveyId} was not found.");
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
    /// <param name="id">Employee ID</param>
    /// <param name="survey">Newly creating Survey. SurveyRequest.AuthorId will be overwrited by Employee ID</param>
    /// <response code="201">Creating Survey is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [Route("{id:guid}/surveys")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult<SurveyResponse> CreateSurvey(Guid id, SurveyRequest survey)
    {
      try
      {
        survey.AuthorId = id;
        Survey newSurvey = _surveyRepository.Add(_mapper.Map<SurveyRequest, Survey>(survey));

        _unitOfWork.Complete();
        _logger.LogInformation($"Survey id={newSurvey.Id} was creating successfully.");

        return CreatedAtAction("GetSurvey", new { id, surveyId = newSurvey.Id }, _mapper.Map<Survey, SurveyResponse>(newSurvey));
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
    /// <param name="id">ID of entity for updating</param>
    /// <param name="surveyId"></param>
    /// <response code="204">Updating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Entity with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}/surveys/{surveyId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult UpdateSurvey(Guid id, Guid surveyId, SurveyRequest request)
    {
      try
      {
        if (_surveyRepository.Get(surveyId) is Survey survey && survey.AuthorId == id)
        {
          request.AuthorId = id;
          Survey updatedEntity = _mapper.Map<SurveyRequest, Survey>(request, survey);

          _unitOfWork.Complete();
          _logger.LogInformation($"Survey id={updatedEntity.Id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Survey with ID={id} was not found.");
          return NotFound($"Survey with ID={id} was not found.");
        }
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
