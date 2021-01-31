using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using Bathhouse.Repositories;
using Bathhouse.ValueTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  public class SurveyController : RichControllerBase<Survey, SurveyResponse, SurveyRequest>
  {
    public SurveyController(
      ILogger<RichControllerBase<Survey, SurveyResponse, SurveyRequest>> logger, 
      IMapper mapper, 
      IUnitOfWork unitOfWork)
      : base(logger, mapper, unitOfWork)
    {
    }

    /// <summary>
    /// Get summary of survey
    /// </summary>
    /// <param name="id">Id of summary</param>
    /// <param name="summaryType">Summary type</param>
    /// <response code="200">Getting all of entities was successful</response>
    /// <response code="404">Survey was not found</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [Route("{id}/summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SurveySummaryResponse> GetSurveySummary(Guid id, [FromQuery]SurveyResultSummaryType summaryType)
    {
      try
      {
        if (_repository.Get(id) is Survey survey)
        {
          _logger.LogInformation($"The survey ID={id} was received successfully.");
          var temp = survey.GetSummary(summaryType);
          return Ok(_mapper.Map<SurveySummary, SurveySummaryResponse>(temp));
        }
        else
        {
          _logger.LogInformation($"The survey ID={id} was not found.");
          return NotFound($"The survey ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting the survey ID={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting the survey ID={id} an exception was fired.");
      }
    }
  }
}
