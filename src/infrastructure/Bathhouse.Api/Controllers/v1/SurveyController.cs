using AutoMapper;
using Bathhouse.Api.Common.Filters;
using Bathhouse.Api.Contracts;
using Bathhouse.Api.Contracts.Models.Requests.v1;
using Bathhouse.Api.Contracts.Models.Responses.v1;
using Bathhouse.Api.Contracts.Models.Queries.v1;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Bathhouse.ValueTypes;
using Chuk.Helpers.AspNetCore;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [HttpGet(ApiRoutes.GetAllSurveys, Name = nameof(ApiRoutes.GetAllSurveys))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<SurveyResponse>> GetAll(
      [FromQuery] PaginationQuery paginationQuery,
      [FromQuery] SurveyFilterQuery filterQuery)
    {
      PaginationFilter paginationFilter = new()
      {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber
      };

      SurveyFilter filter = new(filterQuery);

      var allEntities = _repository.GetAll(
        paginationFilter: paginationFilter,
        filter: filter.Compose(),
        includePropertyNames: new[] { "Author" },
        orderBy: all => all.OrderByDescending(c => c.CreationDate));

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
    [HttpGet(ApiRoutes.GetSurveyById, Name = nameof(ApiRoutes.GetSurveyById))]
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
    [HttpPost(ApiRoutes.CreateSurvey, Name = nameof(ApiRoutes.CreateSurvey))]
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
    [HttpPut(ApiRoutes.UpdateSurvey, Name = nameof(ApiRoutes.UpdateSurvey))]
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
    [HttpDelete(ApiRoutes.DeleteSurvey, Name = nameof(ApiRoutes.DeleteSurvey))]
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

    #region Question

    /// <summary>
    /// Get all of Question in Survey
    /// </summary>
    [HttpGet(ApiRoutes.GetAllQuestionsInSurvey, Name = nameof(ApiRoutes.GetAllQuestionsInSurvey))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<SurveyResponse>> GetAllQuestionsInSurvey(Guid surveyId)
    {
      var allEntities = _unitOfWork.Questions.GetAll(
        filter: q => q.SurveyId == surveyId);

      _logger.LogInformation($"All of Questions in Survey id={surveyId} was got.");

      return Ok(_mapper.Map<IEnumerable<Question>, IEnumerable<QuestionResponse>>(allEntities));
    }

    /// <summary>
    /// Get Question by ID in Survey
    /// </summary>
    /// <param name="surveyId">Exploring Survey ID</param>
    /// <param name="questionId">The question Id in Survey ID</param>
    [HttpGet(ApiRoutes.GetQuestionInSurvey, Name = nameof(ApiRoutes.GetQuestionInSurvey))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<QuestionResponse> GetQuestionInSurvey(Guid surveyId, Guid questionId)
    {
      Question? question = _unitOfWork.Questions.Get(key: questionId);
      if (question is null)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found.");
        return NotFound($"Question with ID={questionId} was not found.");
      }

      if (question.SurveyId != surveyId)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found in survey id={surveyId}.");
        return NotFound($"Question with ID={questionId} was not found in survey id={surveyId}.");
      }

      _logger.LogInformation($"Question id={questionId} was getting successfully.");
      return Ok(_mapper.Map<Question, QuestionResponse>(question));
    }

    /// <summary>
    /// Add Question to Survey.
    /// </summary>
    /// <param name="surveyId">Survey where new question will be added</param>
    /// <param name="request">Newly creating Question</param>
    [HttpPost(ApiRoutes.AddQuestionToSurvey, Name = nameof(ApiRoutes.AddQuestionToSurvey))]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<QuestionResponse> AddQuestionToSurvey(Guid surveyId, QuestionRequest request)
    {
      Survey? survey = _unitOfWork.Surveys.Get(key: surveyId, includePropertyNames: new[] { "Results" });
      if (survey is null)
      {
        _logger.LogInformation($"Survey with ID={surveyId} was not found.");
        return NotFound($"Survey with ID={surveyId} was not found.");
      }

      if (survey.Results.Count > 0)
      {
        _logger.LogInformation($"Impossible to add new question to a survey id={surveyId} with results.");
        return Conflict($"Impossible to add new question to a survey id={surveyId} with results.");
      }

      Question addingQuestion = _mapper.Map<QuestionRequest, Question>(request);
      addingQuestion.SurveyId = survey.Id;

      Question newEntity = _unitOfWork.Questions.Add(addingQuestion);

      _unitOfWork.Complete();
      _logger.LogInformation($"Question id={newEntity.Id} was creating successfully.");

      return CreatedAtAction(
        nameof(GetQuestionInSurvey),
        new { questionId = newEntity.Id, surveyId = newEntity.SurveyId },
        _mapper.Map<Question, QuestionResponse>(newEntity));
    }

    /// <summary>
    /// Update Question in Survey
    /// </summary>
    /// <param name="surveyId">Exploring Survey ID</param>
    /// <param name="questionId">ID of Question for updating</param>
    /// <param name="request">Updating question</param>
    [HttpPut(ApiRoutes.UpdateQuestionInSurvey, Name = nameof(ApiRoutes.UpdateQuestionInSurvey))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
    public ActionResult UpdateQuestionInSurvey(Guid surveyId, Guid questionId, QuestionRequest request)
    {
      Question? entity = _unitOfWork.Questions.Get(key: questionId);
      if (entity is null)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found.");
        return NotFound($"Question with ID={questionId} was not found.");
      }

      if (entity.SurveyId != surveyId)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found in survey id={surveyId}.");
        return NotFound($"Question with ID={questionId} was not found in survey id={surveyId}.");
      }

      _mapper.Map<QuestionRequest, Question>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Question with ID={questionId} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete Question from Survey
    /// </summary>
    /// <param name="surveyId">Exploring Survey ID</param>
    /// <param name="questionId">Question ID</param>
    [HttpDelete(ApiRoutes.DeleteQuestionFromSurvey, Name = nameof(ApiRoutes.DeleteQuestionFromSurvey))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult DeleteQuestionFromSurvey(Guid surveyId, Guid questionId)
    {
      Question? entity = _unitOfWork.Questions.Get(questionId);
      if (entity is null)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found.");
        return NotFound($"Question with ID={questionId} was not found.");
      }

      if (entity.SurveyId != surveyId)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found in survey id={surveyId}.");
        return NotFound($"Question with ID={questionId} was not found in survey id={surveyId}.");
      }

      //clear results
      var results = _unitOfWork.Answers.Where(a => a.QuestionId == questionId);
      _unitOfWork.Answers.DeleteRange(results);

      _unitOfWork.Questions.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Question id={questionId} was deleted successfully.");

      return NoContent();
    }

    #endregion

    /// <summary>
    /// Get summary of survey
    /// </summary>
    /// <param name="surveyId">Id of summary</param>
    /// <param name="summaryType">Summary type</param>
    [HttpGet(ApiRoutes.GetSurveySummary, Name = nameof(ApiRoutes.GetSurveySummary))]
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

      survey.Questions = _unitOfWork.Questions.Where(q => q.SurveyId == survey.Id).ToList();
      survey.Results = _unitOfWork.SurveyResults.GetAll(
        filter: sr => sr.SurveyId == survey.Id,
        includePropertyNames: new[] { "Answers", "Author" }).ToList();

      var summary = survey.GetSummary(summaryType);
      _logger.LogInformation($"The summary of survey ID={surveyId} was received successfully.");
      return Ok(_mapper.Map<SurveySummary, SurveySummaryResponse>(summary));
    }
  }
}
