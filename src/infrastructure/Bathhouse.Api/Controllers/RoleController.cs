using AutoMapper;
using Bathhouse.Contracts;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Microsoft.AspNetCore.Authorization;
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
      return Ok(_mapper.Map<IEnumerable<IdentityRole<Guid>>, IEnumerable<RoleResponse>>(
        _roleManager.Roles.ToList()));
    }

    /// <summary>
    /// Get the role by its ID
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <returns>Role</returns>
    [HttpGet("{roleId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<RoleResponse> GetById(Guid roleId)
    {
      IdentityRole<Guid>? entity = _roleManager.FindByIdAsync(roleId.ToString()).Result;
      if (entity is null)
      {
        _logger.LogInformation($"Role with ID={roleId} was not found.");
        return NotFound($"Role with ID={roleId} was not found.");
      }

      _logger.LogInformation($"Role with ID={roleId} was getting successfully.");
      return Ok(_mapper.Map<IdentityRole<Guid>, RoleResponse>(entity));
    }

    /// <summary>
    /// Create new role
    /// </summary>
    /// <param name="name">Name of the new role.</param>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<RoleResponse> Create(string name)
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
        nameof(GetById),
        new { roleId = newEntity.Id },
        _mapper.Map<IdentityRole<Guid>, RoleResponse>(newEntity));
    }

    /// <summary>
    /// Update name of a role
    /// </summary>
    /// <param name="roleId">Role Id</param>
    /// <param name="newName">New name of the role with Id</param>
    [HttpPut("{roleId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid roleId, string newName)
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

    /// <summary>
    /// Delete Role
    /// </summary>
    /// <param name="roleId">Role Id</param>
    [HttpDelete("{roleId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid roleId)
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

    #region Employee

    /// <summary>
    /// Get all of employees in the role
    /// </summary>
    /// <param name="roleId">Role Id</param>
    [HttpGet("{roleId:guid}/employees", Name = nameof(GetEmployeesInRole))]
    public ActionResult<IEnumerable<EmployeeResponse>> GetEmployeesInRole(Guid roleId)
    {
      IdentityRole<Guid>? entity = _roleManager.FindByIdAsync(roleId.ToString()).Result;
      if (entity is null)
      {
        _logger.LogInformation($"Role with ID={roleId} was not found.");
        return NotFound($"Role with ID={roleId} was not found.");
      }

      _logger.LogInformation($"Role with ID={roleId} was getting successfully.");

      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(
        _userManager.GetUsersInRoleAsync(entity.Name).Result));
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
      var employee = _userManager.FindByIdAsync(employeeId.ToString()).Result;
      var role = _roleManager.FindByIdAsync(roleId.ToString()).Result;

      if (employee is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId}  was not found.");
      }
      _logger.LogInformation($"Employee with ID={employeeId} was getting successfully.");

      if (role is null)
      {
        _logger.LogInformation($"Role with ID={roleId} was not found.");
        return NotFound($"Role with ID={roleId} was not found.");
      }
      _logger.LogInformation($"Role with ID={roleId} was getting successfully.");

      var result = _userManager.AddToRoleAsync(employee, role.Name).Result;

      if (!result.Succeeded)
      {
        _logger.LogInformation($"Employee id={employeeId} was not added to Role with ID={roleId}.");
        return BadRequest(result.Errors);
      }

      return NoContent();
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
      var employee = _userManager.FindByIdAsync(employeeId.ToString()).Result;
      var role = _roleManager.FindByIdAsync(roleId.ToString()).Result;

      if (employee is null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId}  was not found.");
      }
      _logger.LogInformation($"Employee with ID={employeeId} was getting successfully.");

      if (role is null)
      {
        _logger.LogInformation($"Role with ID={roleId} was not found.");
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
    #endregion
  }
}
