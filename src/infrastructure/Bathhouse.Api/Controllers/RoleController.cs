using AutoMapper;
using Bathhouse.Contracts;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Chuk.Helpers.AspNetCore.ApiConvension;
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
  [Authorize(Policy = "Admin")]
  [Route("api/[controller]")]
  [ApiController]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
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

    /// <summary>
    /// Get all of the roles in the system
    /// </summary>
    [HttpGet(Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<IEnumerable<RoleResponse>> GetAll()
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

    /// <summary>
    /// Get the role by its ID
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <returns>Role</returns>
    [HttpGet("{roleId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<RoleResponse> GetById(Guid roleId)
    {
      try
      {
        if (_roleManager.FindByIdAsync(roleId.ToString()).Result is IdentityRole<Guid> entity)
        {
          _logger.LogInformation($"Role with ID={roleId} was getting successfully.");
          return Ok(_mapper.Map<IdentityRole<Guid>, RoleResponse>(entity));
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

    /// <summary>
    /// Create new role
    /// </summary>
    /// <param name="name">Name of the new role.</param>
    /// <returns>Created role.</returns>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<RoleResponse> Create(string name)
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

    /// <summary>
    /// Update name of a role
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <param name="newName">New name of the role with Id</param>
    /// <returns>Nothing</returns>
    [HttpPut("{roleId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid roleId, string newName)
    {
      try
      {
        var entity = _roleManager.FindByIdAsync(roleId.ToString()).Result;
        if (entity is null)
        {
          _logger.LogInformation($"Role with ID={roleId} was not found.");
          return NotFound($"Role with ID={roleId} was not found.");
        }

        entity.Name = newName;
        _roleManager.UpdateNormalizedRoleNameAsync(entity);

        var result = _roleManager.UpdateAsync(entity).Result;
        if (!result.Succeeded)
        {
          _logger.LogInformation($"We have had a problem. Role with id={roleId} was not updated.");
          return BadRequest(result.Errors);
        }

        _logger.LogInformation($"Role id={roleId} was updated successfully.");
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Role id={roleId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Role id={roleId} an exception was fired");
      }
    }

    /// <summary>
    /// Delete Role
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <returns>Nothing</returns>
    [HttpDelete("{roleId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid roleId)
    {
      try
      {
        var entity = _roleManager.FindByIdAsync(roleId.ToString()).Result;

        if (entity is null)
        {
          _logger.LogInformation($"Role with ID={roleId} was not found.");
          return NotFound($"Role with ID={roleId} was not found.");
        }

        if (Constants.GetBuildInNormalizedRoleNames().Contains(entity.NormalizedName))
        {
          _logger.LogInformation($"Trying to delete one of the build-in roles name={entity.NormalizedName}.");
          return Conflict("Trying to delete one of the build-in roles.");
        }

        var result = _roleManager.DeleteAsync(entity).Result;
        if (!result.Succeeded)
        {
          _logger.LogInformation($"We have had a problem. Role with id={roleId} was not deleted.");
          return BadRequest(result.Errors);
        }

        _logger.LogInformation($"Role id={roleId} was deleted successfully.");
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Role id={roleId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Role id={roleId} an exception was fired");
      }
    }

    #region Employee

    /// <summary>
    /// Get all of employees in the role
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <returns>Employee in the role</returns>
    [HttpGet("{roleId:guid}/employees", Name = nameof(GetEmployeesInRole))]
    public ActionResult<IEnumerable<EmployeeResponse>> GetEmployeesInRole(Guid roleId)
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

    /// <summary>
    /// Add employee to the role.
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="employeeId">Employee Id</param>
    [HttpPut("{roleId:guid}/employee", Name = nameof(AddEmployeeToRole))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult AddEmployeeToRole(Guid roleId, Guid employeeId)
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

    /// <summary>
    /// Delete employee from the role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="employeeId">Employee Id</param>
    [HttpDelete("{roleId:guid}/employees/{employeeId:guid}", Name = nameof(DeleteEmployeeFromRole))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult DeleteEmployeeFromRole(Guid roleId, Guid employeeId)
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
