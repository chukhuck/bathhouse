using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Bathhouse.Api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
  public class SurveyController : ControllerBase
  {
    protected readonly IBathhouseUnitOfWork _unitOfWork;
    protected readonly IRepository<Survey, Guid> _repository;
    protected readonly ILogger<SurveyController> _logger;
    protected readonly IMapper _mapper;

    public SurveyController(
      ILogger<SurveyController> logger,
      IMapper mapper,
      IBathhouseUnitOfWork unitOfWork)
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
    [HttpGet(Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<SurveyResponse>> GetAll([FromQuery] PaginationQuery paginationQuery)
    {
      PaginationFilter paginationFilter = new()
      {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber
      };

      var allEntities = _repository.GetAll(
        paginationFilter: paginationFilter, 
        includePropertyNames: new[] { "Author" });

      _logger.LogInformation($"All of Surveys was got.");

      return Ok(new PaginatedResponse<SurveyResponse>()
      {
        Data = _mapper.Map<IEnumerable<Survey>, IEnumerable<SurveyResponse>>(allEntities),
        PageNumber = paginationFilter.IsValid ? paginationFilter.PageNumber : null,
        PageSize = paginationFilter.IsValid ? paginationFilter.PageSize : null
      });
    }

    /// <summary>
    /// Get Survey by ID
    /// </summary>
    /// <param name="surveyId">The Survey ID</param>
    [HttpGet("{surveyId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<SurveyResponse> GetById(Guid surveyId)
    {
      Survey? entity = _repository.Get(key: surveyId, includePropertyNames: new[] { "Author", "Questions" });
      if (entity is null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      _logger.LogInformation($"Survey id={surveyId} was getting successfully.");
      return Ok(_mapper.Map<Survey, SurveyResponse>(entity));
    }

    /// <summary>
    /// Add Survey.
    /// </summary>
    /// <param name="request">Newly creating Survey</param>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<SurveyResponse> Create(SurveyRequest request)
    {
      Survey newEntity = _repository.Add(_mapper.Map<SurveyRequest, Survey>(request));
      newEntity.AuthorId = HttpContext.GetGuidUserId();
      newEntity.CreationDate = DateTime.Now;
      newEntity.Status = SurveyStatus.Work;

      _unitOfWork.Complete();
      _logger.LogInformation($"Survey id={newEntity.Id} was creating successfully.");

      return CreatedAtAction(
        nameof(GetById),
        new { surveyId = newEntity.Id },
        _mapper.Map<Survey, SurveyResponse>(newEntity));
    }

    /// <summary>
    /// Update Survey
    /// </summary>
    /// <param name="surveyId">ID of Survey for updating</param>
    /// <param name="newName">New Name</param>
    /// <param name="newDescription">New Description</param>
    [HttpPut("{surveyId:guid}", Name = ("Update[controller]"))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult Update(Guid surveyId, string? newName, string? newDescription)
    {
      Survey? entity = _repository.Get(key: surveyId);
      if (entity is null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (entity.AuthorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to Survey  ID={surveyId}.");
        return Forbid();
      }

      entity.Name = newName ?? entity.Name;
      entity.Description = newDescription ?? entity.Description;

      _unitOfWork.Complete();
      _logger.LogInformation($"Survey id={surveyId} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete Survey by ID
    /// </summary>
    /// <param name="surveyId">Survey ID</param>
    [HttpDelete("{surveyId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid surveyId)
    {
      Survey? entity = _repository.Get(surveyId);
      if (entity is null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (entity.AuthorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to Survey  ID={surveyId}.");
        return Forbid();
      }

      //clear results
      var results = _unitOfWork.SurveyResults.Where(sr => sr.SurveyId == surveyId);
      _unitOfWork.SurveyResults.DeleteRange(results);

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Survey id={surveyId} was deleted successfully.");

      return NoContent();
    }
    #endregion

    /// <summary>
    /// Get summary of survey
    /// </summary>
    /// <param name="surveyId">Id of summary</param>
    /// <param name="summaryType">Summary type</param>
    [HttpGet("{surveyId:guid}/summary", Name = (nameof(GetSurveySummary)))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<SurveySummaryResponse> GetSurveySummary(Guid surveyId, [FromQuery] SurveyResultSummaryType summaryType)
    {
      Survey? survey = _repository.Get(key: surveyId);
      if (survey is null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (survey.AuthorId != HttpContext.GetGuidUserId())
      {
        _logger.LogInformation($"Unauthorized access to summary of Survey ID={surveyId}.");
        return Forbid();
      }

      _logger.LogInformation($"The survey ID={surveyId} was received successfully.");

      var summary = survey.GetSummary(summaryType);
      _logger.LogInformation($"The summary of survey ID={surveyId} was received successfully.");
      return Ok(_mapper.Map<SurveySummary, SurveySummaryResponse>(summary));
    }
  }
}
