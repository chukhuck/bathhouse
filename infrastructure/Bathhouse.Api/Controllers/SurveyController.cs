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
  public class SurveyController : RichControllerBase<Survey, SurveyModel>
  {
    public SurveyController(ILogger<RichControllerBase<Survey, SurveyModel>> logger, IMapper mapper, ICRUDRepository<Survey> repository)
      : base(logger, mapper, repository)
    {
    }

    /// <summary>
    /// Get all survey result
    /// </summary>
    /// <response code="200">Getting all of entities was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("{id}/Result")]
    public ActionResult<IEnumerable<SurveyResultModel>> GetSurveyResult(Guid id)
    {
      try
      {       
        _logger.LogInformation($"All of entities was getting.");

        return Ok(_mapper.Map<Survey, SurveyResultModel>(_repository.Get(id)));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of entities an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of entities an exception was fired.");
      }
    }
  }
}
