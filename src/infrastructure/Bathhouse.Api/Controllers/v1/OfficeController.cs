using AutoMapper;
using Bathhouse.Contracts;
using Bathhouse.Contracts.Models.Requests.v1;
using Bathhouse.Contracts.Models.Responses.v1;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.AspNetCore;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace Bathhouse.Api.Controllers.v1
{
  [Authorize]
  [ApiController]
  [ApiVersion("1.0")]
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
    [HttpGet(ApiRoutes.GetAllOffices, Name = nameof(ApiRoutes.GetAllOffices))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<OfficeResponse>> GetAll(
      [FromQuery] PaginationQuery paginationQuery,
      [FromQuery] int? officeNumber)
    {
      PaginationFilter paginationFilter = new()
      {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber
      };

      var allEntities = _repository.GetAll(
        paginationFilter: paginationFilter,
        filter: officeNumber == null ? null : office => office.Number == officeNumber,
        orderBy: all => all.OrderBy(c => c.Number));

      _logger.LogInformation($"All of Offices was got.");

      return Ok(new PaginatedResponse<OfficeResponse>()
      {
        Data = _mapper.Map<IEnumerable<Office>, IEnumerable<OfficeResponse>>(allEntities),
        PageNumber = paginationFilter.IsValid ? paginationFilter.PageNumber : null,
        PageSize = paginationFilter.IsValid ? paginationFilter.PageSize : null
      });
    }

    /// <summary>
    /// Get Office by ID
    /// </summary>
    /// <param name="officeId">The Office ID</param>
    [HttpGet(ApiRoutes.GetOfficeById, Name = nameof(ApiRoutes.GetOfficeById))]
    public ActionResult<EmployeeResponse> GetById(Guid officeId)
    {
      Office? entity = _repository.Get(officeId);

      if (entity is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      _logger.LogInformation($"Office id={officeId} was getting successfully.");
      return Ok(_mapper.Map<Office, OfficeResponse>(entity));
    }

    /// <summary>
    /// Add Office.
    /// </summary>
    /// <param name="request">Newly creating Office</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpPost(ApiRoutes.CreateOffice, Name = nameof(ApiRoutes.CreateOffice))]
    public ActionResult<EmployeeResponse> Create(OfficeRequest request)
    {
      Office newEntity = _repository.Add(_mapper.Map<OfficeRequest, Office>(request));

      _unitOfWork.Complete();
      _logger.LogInformation($"Office id= was creating successfully.");

      return CreatedAtAction(
        nameof(GetById),
        new { officeId = newEntity.Id },
        _mapper.Map<Office, OfficeResponse>(newEntity));
    }

    /// <summary>
    /// Update Office
    /// </summary>
    /// <param name="request">Office for updating</param>
    /// <param name="officeId">ID of Office for updating</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpPut(ApiRoutes.UpdateOffice, Name = nameof(ApiRoutes.UpdateOffice))]
    public ActionResult Update(Guid officeId, OfficeRequest request)
    {
      Office? entity = _repository.Get(officeId);

      if (entity is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      _mapper.Map<OfficeRequest, Office>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Office id={officeId} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete Office by ID
    /// </summary>
    /// <param name="officeId">Office ID</param>
    /// <param name="newOfficeId">The ID of Office that will be set for clients of the deleting Office. 
    /// If newOfficeIdForClients equal NULL, then for clients of the deleting Office OfficeId will be set NULL</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpDelete(ApiRoutes.DeleteOffice, Name = nameof(ApiRoutes.DeleteOffice))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public ActionResult Delete(Guid officeId, [FromQuery] Guid? newOfficeId)
    {
      Office? entity = _repository.Get(officeId);
      if (entity is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      if (newOfficeId is not null)
      {
        Office? newOffice = _repository.Get(newOfficeId.Value);

        if (newOffice is null)
        {
          _logger.LogInformation($"New Office with ID={newOfficeId.Value} was not found.");
          return NotFound($"New Office with ID={newOfficeId.Value} was not found.");
        }

        var clients = _unitOfWork.Clients.Where(c => c.OfficeId == officeId);

        foreach (var client in clients)
        {
          client.SetOffice(newOffice);
          _logger.LogInformation($"Set New Office with ID={newOfficeId.Value} to client id={client.Id}.");

        }
      }

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Office id={officeId} was deleted successfully.");

      return NoContent();
    }
    #endregion

    // TODO Add functionality to getting managers for office
    /// <summary>
    /// Get managers of office
    /// </summary>
    /// <param name="officeId">The Office ID</param>
    [HttpGet(ApiRoutes.GetManagersInOffice, Name = nameof(ApiRoutes.GetManagersInOffice))]
    public ActionResult<EmployeeResponse> GetManagersInOffice(Guid officeId)
    {
      Office? office = _repository.Get(officeId);

      if (office is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      _logger.LogInformation($"Office id={officeId} was getting successfully.");
      _logger.LogInformation($"Managers for office id={officeId} was getting successfully.");

      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
    }

    /// <summary>
    /// Get employees of office
    /// </summary>
    /// <param name="officeId">The Office ID</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpGet(ApiRoutes.GetEmployeesInOffice, Name = nameof(ApiRoutes.GetEmployeesInOffice))]
    public ActionResult<EmployeeResponse> GetEmployeesInOffice(Guid officeId)
    {
      Office? office = _repository.Get(key: officeId, includePropertyNames: new[] { "Employees" });

      if (office is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      _logger.LogInformation($"Office id={officeId} was getting successfully.");
      _logger.LogInformation($"Employees for office id={officeId} was getting successfully.");

      return Ok(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(office.Employees));
    }

    /// <summary>
    /// Delete employee of office
    /// </summary>
    /// <param name="officeId">Office ID</param>
    /// <param name="employeeId">ID deleting employee</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpDelete(ApiRoutes.DeleteEmployeeFromOffice, Name = nameof(ApiRoutes.DeleteEmployeeFromOffice))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult DeleteEmployeeFromOffice(Guid officeId, Guid employeeId)
    {
      Office? office = _repository.Get(key: officeId, includePropertyNames: new[] { "Employees" });

      if (office is null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      office.DeleteEmployee(employeeId);

      _unitOfWork.Complete();
      _logger.LogInformation($"Employee of office id={officeId} was deleted successfully.");

      return NoContent();
    }

    /// <summary>
    /// Add employee to office
    /// </summary>
    /// <param name="officeId">Office ID</param>
    /// <param name="employeeId">Employee ID</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpPost(ApiRoutes.AddEmployeeToOffice, Name = nameof(ApiRoutes.AddEmployeeToOffice))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IEnumerable<EmployeeResponse>> AddEmployeeToOffice(Guid officeId, Guid employeeId)
    {
      Office? office = _repository.Get(key: officeId, includePropertyNames: new[] { "Employees" });
      if (office == null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
      }

      Employee? addingEmployee = _employeeRepository.Get(employeeId);
      if (addingEmployee == null)
      {
        _logger.LogInformation($"Employee with ID={employeeId} was not found.");
        return NotFound($"Employee with ID={employeeId} was not found.");
      }

      office.AddEmployee(addingEmployee);

      _unitOfWork.Complete();
      _logger.LogInformation($"Employee id={employeeId} was added to Office ID={officeId} successfully.");

      return NoContent();
    }

    /// <summary>
    /// Set new list of employees to office
    /// </summary>
    /// <param name="officeId">Office ID</param>
    /// <param name="employeeIds">Employee IDs</param>
    [Authorize(Policy = Constants.OfficeModifyPolicy)]
    [HttpPut(ApiRoutes.SetEmployeesToOffice, Name = nameof(ApiRoutes.SetEmployeesToOffice))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
    public ActionResult<IEnumerable<EmployeeResponse>> SetEmployeesToOffice(
      Guid officeId,
      [FromBody] IEnumerable<Guid> employeeIds)
    {
      Office? office = _repository.Get(key: officeId, includePropertyNames: new[] { "Employees" });
      if (office == null)
      {
        _logger.LogInformation($"Office with ID={officeId} was not found.");
        return NotFound($"Office with ID={officeId} was not found.");
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

        _logger.LogInformation($"Employee id={employeeId} was found.");
        office.AddEmployee(addingEmployee);
      }

      _unitOfWork.Complete();
      _logger.LogInformation($"Employees was added to Office ID={officeId} successfully.");

      return NoContent();
    }
  }
}
