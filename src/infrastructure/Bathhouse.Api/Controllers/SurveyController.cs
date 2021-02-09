using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Bathhouse.ValueTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SurveyController : ControllerBase
  {

    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IRepository<Survey, Guid> _repository;
    protected readonly ILogger<SurveyController> _logger;
    protected readonly IMapper _mapper;

    public SurveyController(
      ILogger<SurveyController> logger, 
      IMapper mapper, 
      IUnitOfWork unitOfWork)
    {
      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Survey, Guid>();
    }

    #region CRUD endpoints

    /// <summary>
    /// Get all of Surveys
    /// </summary>
    /// <response code="200">Getting all of Surveys was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<IEnumerable<SurveyResponse>> Get()
    {
      try
      {
        var allEntities = _repository.GetAll(includePropertyNames: new[] { "Author"});
        _logger.LogInformation($"All of Surveys was got.");

        return Ok(_mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyResponse>>(allEntities));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of Surveys an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of Surveys an exception was fired.");
      }
    }

    /// <summary>
    /// Get Survey by ID
    /// </summary>
    /// <param name="id">The Survey ID</param>
    /// <response code="404">Survey with current ID is not found</response>
    /// <response code="200">Getting Survey is successul</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<SurveyResponse> GetById(Guid id)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Author", "Questions" }) is Survey entity)
        {
          _logger.LogInformation($"Survey id={id} was getting successfully.");
          return Ok(_mapper.Map<Survey, SurveyResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Survey id={id} was received.");
          return NotFound($"Survey with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Survey id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Survey id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Add Survey.
    /// </summary>
    /// <param name="request">Newly creating Survey</param>
    /// <response code="201">Creating Survey is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<SurveyResponse> Create(SurveyRequest request)
    {
      try
      {
        Survey newEntity = _repository.Add(_mapper.Map<SurveyRequest, Survey>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"Survey id={newEntity.Id} was creating successfully.");

        return CreatedAtAction("GetById", new { id = newEntity.Id }, _mapper.Map<Survey, SurveyResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating Survey an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating Survey an exception was fired");
      }
    }

    /// <summary>
    /// Update Survey
    /// </summary>
    /// <param name="request">Survey for updating</param>
    /// <param name="id">ID of Survey for updating</param>
    /// <response code="204">Updating Survey is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Entity with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult Update(Guid id, SurveyRequest request)
    {
      try
      {
        if (_repository.Get(id, includePropertyNames: new[] { "Questions"}) is Survey entity)
        {
          Survey updatedEntity = _mapper.Map<SurveyRequest, Survey>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"Survey id={id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Survey with ID={id} was not found.");
          return NotFound($"Survey with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Survey an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Survey an exception was fired");
      }
    }

    /// <summary>
    /// Delete Survey by ID
    /// </summary>
    /// <param name="id">Survey ID</param>
    /// <response code="404">Survey with current ID is not found</response>
    /// <response code="204">Deleting Survey is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpDelete]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual IActionResult Delete(Guid id)
    {
      try
      {
        Survey? entity = _repository.Get(id);
        if (entity is null)
        {
          _logger.LogInformation($"Survey with ID={id} was not found.");
          return NotFound($"Survey with ID={id} was not found.");
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Survey id={id} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Survey id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Survey id={id} an exception was fired");
      }
    }
    #endregion

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
          var summary = survey.GetSummary(summaryType);
          return Ok(_mapper.Map<SurveySummary, SurveySummaryResponse>(summary));
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
