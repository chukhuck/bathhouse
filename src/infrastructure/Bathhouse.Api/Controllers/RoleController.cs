using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RoleController : ControllerBase
  {
    protected readonly RoleManager<IdentityRole<Guid>> _roleManager;
    protected readonly UserManager<Employee> _userManager;
    protected readonly ILogger<RoleController> _logger;
    protected readonly IMapper _mapper;

    public RoleController(
      RoleManager<IdentityRole<Guid>> roleManager,
      UserManager<Employee> userManager,
      ILogger<RoleController> logger,
      IMapper mapper)
    {
      _roleManager = roleManager;
      _userManager = userManager;
      _logger = logger;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<IEnumerable<RoleResponse>> Get()
    {
      try
      {
        return Ok(_mapper.Map<IEnumerable<IdentityRole<Guid>>, IEnumerable<RoleResponse>>(_roleManager.Roles.ToList()));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of Employees an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of Employees an exception was fired.");
      }
    }

    [HttpGet()]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<RoleResponse> GetById(Guid id)
    {
      try
      {
        if (_roleManager.FindByIdAsync(id.ToString()).Result is IdentityRole<Guid> entity)
        {
          _logger.LogInformation($"Role with ID={id} was getting successfully.");
          return Ok(_mapper.Map<IdentityRole<Guid>, RoleResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Role with ID={id} was received.");
          return NotFound($"Role with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Role with ID={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Role with ID={id} an exception was fired");
      }
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<RoleResponse> Create(string name)
    {
      try
      {
        IdentityRole<Guid> newEntity = new IdentityRole<Guid>(name);
        var result = _roleManager.CreateAsync(newEntity).Result;

        if (!result.Succeeded)
        {
          _logger.LogError($"We have had a problem. Role with name={name} was not created.");
          return BadRequest(result.Errors);
        }

        _logger.LogInformation($"Role name={name} was created successfully.");
        return CreatedAtAction(
          "GetById",
          new { id = newEntity.Id },
          _mapper.Map<IdentityRole<Guid>, RoleResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating Role with name={name} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating Role with name={name} an exception was fired");
      }
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult Update(Guid id, string newName)
    {
      try
      {
        var entity = _roleManager.FindByIdAsync(id.ToString()).Result;
        if (entity is null)
        {
          _logger.LogInformation($"Role with ID={id} was not found.");
          return NotFound($"Role with ID={id} was not found.");
        }

        entity.Name = newName;
        _roleManager.UpdateNormalizedRoleNameAsync(entity);

        var result = _roleManager.UpdateAsync(entity).Result;
        if (!result.Succeeded)
        {
          _logger.LogInformation($"We have had a problem. Role with id={id} was not updated.");
          return BadRequest(result.Errors);
        }

        _logger.LogInformation($"Role id={id} was updated successfully.");
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Role id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Role id={id} an exception was fired");
      }
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual IActionResult Delete(Guid id)
    {
      try
      {
        var entity = _roleManager.FindByIdAsync(id.ToString()).Result;
        if (entity is null)
        {
          _logger.LogInformation($"Role with ID={id} was not found.");
          return NotFound($"Role with ID={id} was not found.");
        }

        var result = _roleManager.DeleteAsync(entity).Result;
        if (!result.Succeeded)
        {
          _logger.LogInformation($"We have had a problem. Role with id={id} was not deleted.");
          return BadRequest(result.Errors);
        }

        _logger.LogInformation($"Role id={id} was deleted successfully.");
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Role id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Role id={id} an exception was fired");
      }
    }

    #region Employee

    [HttpGet()]
    [Route("{roleId:guid}/employees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<RoleResponse> GetEmployeesInRole(Guid roleId)
    {
      try
      {
        if (_roleManager.FindByIdAsync(roleId.ToString()).Result is IdentityRole<Guid> entity)
        {
          _logger.LogInformation($"Role with ID={roleId} was getting successfully.");

          return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
            _userManager.GetUsersInRoleAsync(entity.Name).Result));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Role with ID={roleId} was received.");
          return NotFound($"Role with ID={roleId} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Role with ID={roleId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Role with ID={roleId} an exception was fired");
      }
    }

    [HttpPost()]
    [Route("{roleId:guid}/employees/{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<RoleResponse> AddEmployeeToRole(Guid roleId, Guid employeeId)
    {
      try
      {
        var employee = _userManager.FindByIdAsync(employeeId.ToString()).Result;
        var role = _roleManager.FindByIdAsync(roleId.ToString()).Result;

        if (employee is null)
        {
          _logger.LogInformation($"Employee with ID={employeeId} was not found.");
          return NotFound($"Employee with ID={employeeId}  was not found.");
        }
        _logger.LogInformation($"Role with ID={employeeId} was getting successfully.");

        if (role is null)
        {
          _logger.LogInformation($"Request on getting unexisting Role with ID={roleId} was received.");
          return NotFound($"Role with ID={roleId} was not found.");
        }
        _logger.LogInformation($"Role with ID={roleId} was getting successfully.");

        var result =_userManager.AddToRoleAsync(employee, role.Name).Result;

        if (!result.Succeeded)
        {
          _logger.LogInformation($"Employee id={employeeId}was not added to Role with ID={roleId}.");
          return BadRequest(result.Errors);
        }

        return NoContent();

      }
      catch (Exception ex)
      {
        _logger.LogError($"While adding Employee id={employeeId} to Role with ID={roleId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While  adding Employee id={employeeId} to Role with ID={roleId} an exception was fired");
      }
    }

    [HttpDelete()]
    [Route("{roleId:guid}/employees/{employeeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<RoleResponse> DeleteEmployeeFromRole(Guid roleId, Guid employeeId)
    {
      try
      {
        var employee = _userManager.FindByIdAsync(employeeId.ToString()).Result;
        var role = _roleManager.FindByIdAsync(roleId.ToString()).Result;

        if (employee is null)
        {
          _logger.LogInformation($"Employee with ID={employeeId} was not found.");
          return NotFound($"Employee with ID={employeeId}  was not found.");
        }
        _logger.LogInformation($"Role with ID={employeeId} was getting successfully.");

        if (role is null)
        {
          _logger.LogInformation($"Request on getting unexisting Role with ID={roleId} was received.");
          return NotFound($"Role with ID={roleId} was not found.");
        }
        _logger.LogInformation($"Role with ID={roleId} was got successfully.");

        var result = _userManager.RemoveFromRoleAsync(employee, role.Name).Result;

        if (!result.Succeeded)
        {
          _logger.LogInformation($"Employee id={employeeId} was not deleted from Role with ID={roleId}.");
          return BadRequest(result.Errors);
        }

        return NoContent();

      }
      catch (Exception ex)
      { 
        _logger.LogError($"While deleting Employee id={employeeId} from Role with ID={roleId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Employee id={employeeId} from Role with ID={roleId} an exception was fired");
      }
    }
    #endregion
  }
}
