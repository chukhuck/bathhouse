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
  public class OfficeController : RichControllerBase<Office, OfficeResponse, OfficeRequest>
  {
    public OfficeController(ILogger<RichControllerBase<Office, OfficeResponse, OfficeRequest>> logger, IMapper mapper, ICRUDRepository<Office> repository) 
      : base(logger, mapper, repository)
    {
    }

    /// <summary>
    /// Get manager of office with ID
    /// </summary>
    /// <param name="id">The entity ID</param>
    /// <response code="404">Office with current ID is not found of there is no manager at the office with current ID</response>
    /// <response code="200">Getting entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType((int)StatusCodes.Status404NotFound)]
    public virtual ActionResult<EmployeeResponse> GetManager(Guid id)
    {
      try
      {
        if (!_repository.Exist(id))
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }

        Office entity = _repository.Get(id);
        _logger.LogInformation($"Office id={id} was getting successfully.");

        if (entity.Manager == null)
        {
          _logger.LogInformation($"There is no manager at the office with ID={id}");
          return NotFound($"There is no manager at the office with ID={id}");
        }

        return Ok(_mapper.Map<Employee, EmployeeResponse>(entity.Manager));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting manager of office id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting manager of office id={id} an exception was fired");
      }
    }
  }
}
