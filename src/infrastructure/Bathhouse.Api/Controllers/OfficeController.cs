using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
  public class OfficeController : ControllerBase
  {
    readonly IRepository<Employee, Guid> _employeeRepository;
    protected readonly IBathhouseUnitOfWork _unitOfWork;
    protected readonly IRepository<Office, Guid> _repository;
    protected readonly ILogger<OfficeController> _logger;
    protected readonly IMapper _mapper;

    public OfficeController(
      ILogger<OfficeController> logger,
      IMapper mapper,
      IBathhouseUnitOfWork unitOfWork)
    {
      _employeeRepository = unitOfWork.Repository<Employee, Guid>();
      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Office, Guid>();
    }

    #region CRUD endpoints
    /// <summary>
    /// Get all of Offices
    /// </summary>
    [HttpGet(Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<IEnumerable<OfficeResponse>> GetAll()
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
    /// <param name="officeId">The Office ID</param>
    [HttpGet("{officeId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<EmployeeResponse> GetById(Guid officeId)
    {
      try
      {
        if (_repository.Get(officeId) is Office entity)
        {
          _logger.LogInformation($"Office id={officeId} was getting successfully.");
          return Ok(_mapper.Map<Office, OfficeResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Office id={officeId} was received.");
          return NotFound($"Office with ID={officeId} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Office id={officeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Office id={officeId} an exception was fired");
      }
    }

    /// <summary>
    /// Add Office.
    /// </summary>
    /// <param name="request">Newly creating Office</param>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<EmployeeResponse> Create(OfficeRequest request)
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
    /// <param name="officeId">ID of Office for updating</param>
    [HttpPut("{officeId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid officeId, OfficeRequest request)
    {
      try
      {
        if (_repository.Get(officeId) is Office entity)
        {
          Office updatedEntity = _mapper.Map<OfficeRequest, Office>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"Office id={officeId} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Office with ID={officeId} was not found.");
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
    /// <param name="officeId">Office ID</param>
    /// <param name="newOfficeIdForClients">The ID of Office that will be set for clients of the deleting Office. 
    /// If newOfficeIdForClients equal NULL, then for clients of the deleting Office OfficeId will be set NULL</param>
    [HttpDelete("{officeId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid officeId, [FromQuery] Guid? newOfficeIdForClients)
    {
      try
      {
        Office? entity = _repository.Get(officeId);
        if (entity is null)
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Office with ID={officeId} was not found.");
        }

        if (newOfficeIdForClients is not null)
        {
          Office? newOffice = _repository.Get(newOfficeIdForClients.Value);

          if (newOffice is null)
          {
            _logger.LogInformation($"New Office with ID={newOfficeIdForClients.Value} was not found.");
            return NotFound($"New Office with ID={newOfficeIdForClients.Value} was not found.");
          }

          var clients = _unitOfWork.Clients.Where(c => c.OfficeId == officeId);

          foreach (var client in clients)
          {
            client.SetOffice(newOffice);
          }
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Office id={officeId} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Office id={officeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Office id={officeId} an exception was fired");
      }
    }
    #endregion

    // TODO 
    /// <summary>
    /// Get managers of office
    /// </summary>
    /// <param name="officeId">The Office ID</param>
    [HttpGet("{officeId:guid}/managers", Name = nameof(GetManagersInOffice))]
    public ActionResult<EmployeeResponse> GetManagersInOffice(Guid officeId)
    {
      try
      {
        if (_repository.Get(officeId) is Office office)
        {
          _logger.LogInformation($"Office id={officeId} was getting successfully.");
          _logger.LogInformation($"Managers for office id={officeId} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
        }
        else
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Office with ID={officeId} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting manager of office id={officeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting manager of office id={officeId} an exception was fired");
      }
    }

    /// <summary>
    /// Get employees of office
    /// </summary>
    /// <param name="officeId">The Office ID</param>
    [HttpGet("{officeId:guid}/employees", Name = nameof(GetEmployeesInOffice))]
    public ActionResult<EmployeeResponse> GetEmployeesInOffice(Guid officeId)
    {
      try
      {
        if (_repository.Get(key: officeId, includePropertyNames: new[] { "Employees"}) is Office office)
        {
          _logger.LogInformation($"Office id={officeId} was getting successfully.");
          _logger.LogInformation($"Employees for office id={officeId} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
        }
        else
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Office with ID={officeId} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting manager of office id={officeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting manager of office id={officeId} an exception was fired");
      }
    }

    /// <summary>
    /// Delete employee of office
    /// </summary>
    /// <param name="officeId">Office ID</param>
    /// <param name="employeeId">ID deleting employee</param>
    [HttpDelete("{officeId:guid}/employees/{employeeId:guid}", Name = nameof(DeleteEmployeeFromOffice))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public virtual IActionResult DeleteEmployeeFromOffice(Guid officeId, Guid employeeId)
    {
      try
      {
        if (_repository.Get(key: officeId, includePropertyNames: new[] { "Employees" }) is Office office)
        {
          office.DeleteEmployee(employeeId);

          _unitOfWork.Complete();
          _logger.LogInformation($"Employee of office id={officeId} was deleted successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Office with ID={officeId} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting manager for office  id={officeId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting manager for office  id={officeId}  an exception was fired");
      }
    }

    /// <summary>
    /// Add employee to office
    /// </summary>
    /// <param name="officeId">Office ID</param>
    /// <param name="employeeId">Employee ID</param>
    [HttpPost("{officeId:guid}/employees", Name = nameof(AddEmployeeToOffice))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public virtual ActionResult<IEnumerable<EmployeeResponse>> AddEmployeeToOffice(Guid officeId, Guid employeeId)
    {
      try
      {
        if (_repository.Get(key: officeId, includePropertyNames: new[] { "Employees" }) is Office office 
          && _employeeRepository.Get(employeeId) is Employee addingEmployee)
        {
          office.AddEmployee(addingEmployee);

          _unitOfWork.Complete();
          _logger.LogInformation($"Employee id={employeeId} was added to Office ID={officeId} successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Office with ID={officeId} or Employee with ID={employeeId} was not found.");
          return NotFound($"Office with ID={officeId} or Employee with ID={employeeId} was not found.");
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
    /// <param name="officeId">Office ID</param>
    /// <param name="employeeIds">Employee IDs</param>
    /// <response code="201">Adding employee is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Office with current ID or one of Employee IDs is not found</response>
    [HttpPut("{officeId:guid}/employees", Name = nameof(SetEmployeesToOffice))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public virtual ActionResult<IEnumerable<EmployeeResponse>> SetEmployeesToOffice(Guid officeId, [FromBody]IEnumerable<Guid> employeeIds)
    {
      try
      {
        Office? office = _repository.Get(key: officeId, includePropertyNames: new[] { "Employees" });
        if (office == null)
        {
          _logger.LogInformation($"Office with ID={officeId} was not found.");
          return NotFound($"Employee with ID={officeId} was not found.");
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
        _logger.LogInformation($"Employees was added to Office ID={officeId} successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While adding employee to office an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While adding employee to office an exception was fired");
      }
    }
  }
}
