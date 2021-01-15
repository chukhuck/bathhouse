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
using System.Threading.Tasks;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  public class EmployeeController : RichControllerBase<Employee, EmployeeResponse, EmployeeRequest>
  {
    public EmployeeController(
      ILogger<RichControllerBase<Employee, EmployeeResponse, EmployeeRequest>> logger,
      IMapper mapper,
      ICRUDRepository<Employee> repository)
      : base(logger, mapper, repository)
    {
    }

    /// <summary>
    /// Get all of the directors in the system
    /// </summary>
    /// <response code="200">Getting the directors is successul.</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("directors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EmployeeResponse> GetDirectors()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.Director)));
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
    /// <response code="200">Getting employees is successul.</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("employees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EmployeeResponse> GetEmployees()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.Employee)));
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
    /// <response code="200">Getting the managers is successul.</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("managers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EmployeeResponse> GetManagers()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.Manager)));
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
    /// <response code="200">Getting the tech supporters is successul.</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("techsupporters")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EmployeeResponse> GetTechSupporters()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(_repository.Where(e => e.Type == EmployeeType.TechnicalSupport)));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting the tech supporters an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While the tech supporters directors an exception was fired");
      }
    }

    /// <summary>
    /// Get offices with ID
    /// </summary>
    /// <param name="id">The Employee ID</param>
    /// <response code="404">Employee with current ID is not found</response>
    /// <response code="200">Getting office is successul.</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/offices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EmployeeResponse> GetOffices(Guid id)
    {
      try
      {
        if (_repository.Get(id) is Employee employee)
        {
          _logger.LogInformation($"Employee id={id} was getting successfully.");
          _logger.LogInformation($"Office for employee id={id} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(employee.GetOffices()));
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

  }
}
