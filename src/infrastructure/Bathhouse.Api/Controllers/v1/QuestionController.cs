using AutoMapper;
using Bathhouse.Contracts.v1.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
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
    public ActionResult<QuestionResponse> GetQuestionById(Guid questionId)
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
