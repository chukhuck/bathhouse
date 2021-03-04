using AutoMapper;
using Bathhouse.Contracts.v1.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
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
using System.Threading.Tasks;

namespace Bathhouse.Api.Controllers.v1
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
  public class QuestionController : ControllerBase
  {
    protected readonly IBathhouseUnitOfWork _unitOfWork;
    protected readonly IRepository<Question, Guid> _repository;
    protected readonly ILogger<QuestionController> _logger;
    protected readonly IMapper _mapper;

    public QuestionController(
        ILogger<QuestionController> logger,
        IMapper mapper,
        IBathhouseUnitOfWork unitOfWork)
    {
      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Question, Guid>();
    }

    /// <summary>
    /// Get Question by ID
    /// </summary>
    /// <param name="questionId">The question Id in Survey ID</param>
    [HttpGet("{questionId:guid}", Name = ("Get[controller]ById"))]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<QuestionResponse> GetById(Guid questionId)
    {
      Question? question = _unitOfWork.Questions.Get(key: questionId);
      if (question is null)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found.");
        return NotFound($"Question with ID={questionId} was not found.");
      }

      _logger.LogInformation($"Question id={questionId} was getting successfully.");
      return Ok(_mapper.Map<Question, QuestionResponse>(question));
    }

    /// <summary>
    /// Add Question.
    /// </summary>
    /// <param name="surveyId">Survey where new question will be added</param>
    /// <param name="request">Newly creating Question</param>
    [HttpPost(Name = ("Create[controller]"))]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<QuestionResponse> Create(Guid surveyId, QuestionRequest request)
    {
      Survey? survey = _unitOfWork.Surveys.Get(key: surveyId, includePropertyNames: new[] { "Questions", "Results"});
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
        nameof(GetById),
        new { questionId = newEntity.Id },
        _mapper.Map<Question, QuestionResponse>(newEntity));
    }

    /// <summary>
    /// Update Question
    /// </summary>
    /// <param name="questionId">ID of Question for updating</param>
    /// <param name="request">Updating question</param>
    [HttpPut("{questionId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid questionId, QuestionRequest request)
    {
      Question? entity = _repository.Get(key: questionId);
      if (entity is null)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found.");
        return NotFound($"Question with ID={questionId} was not found.");
      }

      _mapper.Map<QuestionRequest, Question>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Question with ID={questionId} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete Question by ID
    /// </summary>
    /// <param name="questionId">Question ID</param>
    [HttpDelete("{questionId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid questionId)
    {
      Question? entity = _repository.Get(questionId);
      if (entity is null)
      {
        _logger.LogInformation($"Question with ID={questionId} was not found.");
        return NotFound($"Question with ID={questionId} was not found.");
      }

      //clear results
      var results = _unitOfWork.Answers.Where(a => a.QuestionId == questionId);
      _unitOfWork.Answers.DeleteRange(results);

      _repository.Delete(entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Question id={questionId} was deleted successfully.");

      return NoContent();
    }
  }
}
