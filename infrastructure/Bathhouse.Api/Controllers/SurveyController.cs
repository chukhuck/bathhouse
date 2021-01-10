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
  public class SurveyController : RichControllerBase<Survey, SurveyResponse, SurveyRequest>
  {
    public SurveyController(ILogger<RichControllerBase<Survey, SurveyResponse, SurveyRequest>> logger, IMapper mapper, ICRUDRepository<Survey> repository)
      : base(logger, mapper, repository)
    {
    }

    /// <summary>
    /// Get summary of survey
    /// </summary>
    /// <param name="id">Id of summary</param>
    /// <param name="summaryType">Summary type</param>
    /// <response code="200">Getting all of entities was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("{id}/Result")]
    public ActionResult<BaseSurveyResultSummaryResponse> GetSurveyResult(Guid id, [FromQuery]SurveyResultSummaryType summaryType)
    {
      try
      {       
        _logger.LogInformation($"All of entities was getting.");

        return Ok(_mapper.Map<BaseSurveyResultSummary, BaseSurveyResultSummaryResponse>( _repository.Get(id).GetSummary(summaryType)));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of entities an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of entities an exception was fired.");
      }
    }
  }
}
