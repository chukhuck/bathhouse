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
    /// <param name="id">The Office ID</param>
    /// <response code="404">Office with current ID is not found of there is no manager at the office with current ID</response>
    /// <response code="200">Getting manager is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}/manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType((int)StatusCodes.Status404NotFound)]
    public ActionResult<EmployeeResponse> GetManager(Guid id)
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

    /// <summary>
    /// Delete manager of office with ID
    /// </summary>
    /// <param name="id">Office ID</param>
    /// <response code="404">Office with current ID is not found</response>
    /// <response code="204">Deleting manager is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [Route("{id:guid}/manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
    [ProducesResponseType((int)StatusCodes.Status404NotFound)]
    public virtual IActionResult DeleteManager(Guid id)
    {
      try
      {
        if (!_repository.Exist(id))
        {
          _logger.LogInformation($"Office with ID={id} was not found.");
          return NotFound($"Office with ID={id} was not found.");
        }

        var office =_repository.Get(id);
        office.ClearManager();
        _logger.LogInformation($"Manager of office id={id} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting manager for office  id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting manager for office  id={id}  an exception was fired");
      }
    }
  }
}
