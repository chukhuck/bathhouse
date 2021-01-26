using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  public class OfficeController : RichControllerBase<Office, OfficeResponse, OfficeRequest>
  {
    readonly ICRUDRepository<Employee> _employeeRepository;

    public OfficeController(
      ILogger<RichControllerBase<Office, OfficeResponse, OfficeRequest>> logger,
      IMapper mapper,
      ICRUDRepository<Office> repository,
      ICRUDRepository<Employee> employeeRepository)
      : base(logger, mapper, repository)
    {
      _employeeRepository = employeeRepository;
    }

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

          return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.GetManagers()));
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
        if (_repository.Get(id) is Office office)
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
        if (_repository.Get(id) is Office office)
        {
          office.DeleteEmployee(employeeId);

          _repository.SaveChanges();
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
        if (_repository.Get(id) is Office office && _employeeRepository.Get(employeeId) is Employee addingEmployee)
        {
          office.AddEmployee(addingEmployee);

          if (_repository.SaveChanges())
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
        Office? office = _repository.Get(id);
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

        if (_repository.SaveChanges())
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
