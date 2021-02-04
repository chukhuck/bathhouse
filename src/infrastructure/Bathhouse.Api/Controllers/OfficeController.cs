using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using Bathhouse.Repositories;
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
  public class OfficeController : ControllerBase
  {
    readonly IRepository<Employee> _employeeRepository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IRepository<Office> _repository;

    protected readonly ILogger<OfficeController> _logger;

    protected readonly IMapper _mapper;

    public OfficeController(
      ILogger<OfficeController> logger,
      IMapper mapper,
      IUnitOfWork unitOfWork)
    {
      _employeeRepository = unitOfWork.Repository<Employee>();
      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Office>();
    }

    #region CRUD endpoints
    /// <summary>
    /// Get all of Offices
    /// </summary>
    /// <response code="200">Getting all of Offices was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<IEnumerable<OfficeResponse>> Get()
    {
      try
      {
        var allEntities = _repository.GetAll(orderBy: all => all.OrderBy(c => c.Number));
        _logger.LogInformation($"All of Offices was got.");

        return Ok(_mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(allEntities));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of Offices an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of Offices an exception was fired.");
      }
    }

    /// <summary>
    /// Get Office by ID
    /// </summary>
    /// <param name="id">The Office ID</param>
    /// <response code="404">Office with current ID is not found</response>
    /// <response code="200">Getting Office is successul</response>
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
        if (_repository.Get(id) is Office entity)
        {
          _logger.LogInformation($"Office id={id} was getting successfully.");
          return Ok(_mapper.Map<Office, OfficeResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Office id={id} was received.");
          return NotFound($"Office with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Office id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Office id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Add Office.
    /// </summary>
    /// <param name="request">Newly creating Office</param>
    /// <response code="201">Creating Office is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<EmployeeResponse> Create(OfficeRequest request)
    {
      try
      {
        Office newEntity = _repository.Add(_mapper.Map<OfficeRequest, Office>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"Office id= was creating successfully.");

        return CreatedAtAction("GetById", new { id = newEntity.Id }, _mapper.Map<Office, OfficeResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating Office an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating Office an exception was fired");
      }
    }

    /// <summary>
    /// Update Office
    /// </summary>
    /// <param name="request">Office for updating</param>
    /// <param name="id">ID of Office for updating</param>
    /// <response code="204">Updating Office is successul</response>
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
    public virtual ActionResult Update(Guid id, OfficeRequest request)
    {
      try
      {
        if (_repository.Get(id) is Office entity)
        {
          Office updatedEntity = _mapper.Map<OfficeRequest, Office>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"Office id={id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Office an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Office an exception was fired");
      }
    }

    /// <summary>
    /// Delete Office by ID
    /// </summary>
    /// <param name="id">Office ID</param>
    /// <response code="404">Office with current ID is not found</response>
    /// <response code="204">Deleting Office is successul</response>
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
        Office? entity = _repository.Get(id);
        if (entity is null)
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Office id={id} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Office id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Office id={id} an exception was fired");
      }
    }
    #endregion

    // TODO 
    /// <summary>
    /// Get managers of office
    /// </summary>
    /// <param name="id">The Office ID</param>
    /// <response code="404">Office with current ID is not found</response>
    /// <response code="200">Getting managers is successul.</response>
    /// <response code="400">If the item is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/managers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EmployeeResponse> GetManagers(Guid id)
    {
      try
      {
        if (_repository.Get(id) is Office office)
        {
          _logger.LogInformation($"Office id={id} was getting successfully.");
          _logger.LogInformation($"Managers for office id={id} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
        }
        else
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting manager of office id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting manager of office id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Get employees of office
    /// </summary>
    /// <param name="id">The Office ID</param>
    /// <response code="404">Office with current ID is not found</response>
    /// <response code="200">Getting employees is successul.</response>
    /// <response code="400">If the item is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/employees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EmployeeResponse> GetEmployees(Guid id)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Employees"}) is Office office)
        {
          _logger.LogInformation($"Office id={id} was getting successfully.");
          _logger.LogInformation($"Employees for office id={id} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
        }
        else
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting manager of office id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting manager of office id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Delete employee of office
    /// </summary>
    /// <param name="id">Office ID</param>
    /// <param name="employeeId">ID deleting employee</param>
    /// <response code="404">Office with current ID is not found</response>
    /// <response code="204">Deleting employee is successul</response>
    /// <response code="400">If the item is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [Route("{id:guid}/employees")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual IActionResult DeleteEmployee(Guid id, Guid employeeId)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Employees" }) is Office office)
        {
          office.DeleteEmployee(employeeId);

          _unitOfWork.Complete();
          _logger.LogInformation($"Employee of office id={id} was deleted successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting manager for office  id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting manager for office  id={id}  an exception was fired");
      }
    }

    /// <summary>
    /// Add employee to office
    /// </summary>
    /// <param name="id">Office ID</param>
    /// <param name="employeeId">Employee ID</param>
    /// <response code="201">Adding employee is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Office of Employee was not found</response>
    [HttpPost]
    [Route("{id:guid}/employees")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult<IEnumerable<EmployeeResponse>> AddEmployee(Guid id, Guid employeeId)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Employees" }) is Office office 
          && _employeeRepository.Get(employeeId) is Employee addingEmployee)
        {
          office.AddEmployee(addingEmployee);

          _unitOfWork.Complete();
          _logger.LogInformation($"Employee id={employeeId} was added to Office ID={id} successfully.");

          return CreatedAtAction(nameof(GetEmployees), new {id }, _mapper.Map<ICollection<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
        }
        else
        {
          _logger.LogInformation($"Office with ID={id} or Employee with ID={employeeId} was not found.");
          return NotFound($"Office with ID={id} or Employee with ID={employeeId} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While adding employee to office an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While adding employee to office an exception was fired");
      }
    }

    /// <summary>
    /// Set new list of employees to office
    /// </summary>
    /// <param name="id">Office ID</param>
    /// <param name="employeeIds">Employee IDs</param>
    /// <response code="201">Adding employee is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Office with current ID or one of Employee IDs is not found</response>
    [HttpPut]
    [Route("{id:guid}/employees")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual ActionResult<IEnumerable<EmployeeResponse>> SetEmployees(Guid id, [FromBody]IEnumerable<Guid> employeeIds)
    {
      try
      {
        Office? office = _repository.Get(key: id, includePropertyNames: new[] { "Employees" });
        if (office == null)
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Employee with ID={id} was not found.");
        }
        
        office.Employees.Clear();

        foreach (var employeeId in employeeIds)
        {
          Employee? addingEmployee = _employeeRepository.Get(employeeId);

          if (addingEmployee == null)
          {
            _logger.LogInformation($"Employee with ID={employeeId} was not found.");
            return NotFound($"Employee with ID={employeeId} was not found.");
          }

          office.Employees.Add(addingEmployee);
          _logger.LogInformation($"Employee id={employeeId} was found.");
        }

        _unitOfWork.Complete();
        _logger.LogInformation($"Employees was added to Office ID={id} successfully.");

        return CreatedAtAction(nameof(GetEmployees), new { id }, _mapper.Map<ICollection<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While adding employee to office an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While adding employee to office an exception was fired");
      }
    }
  }
}
