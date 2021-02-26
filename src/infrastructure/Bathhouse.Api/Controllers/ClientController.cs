using AutoMapper;
using Bathhouse.Contracts.Models;
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

namespace Bathhouse.Api.Controllers
{
  [Authorize]
  [Route("[controller]")]
  [ApiController]
  [Produces(MediaTypeNames.Application.Json)]
  [Consumes(MediaTypeNames.Application.Json)]
  public class ClientController : ControllerBase
  {
    protected readonly IRepository<Office, Guid> _officeRepository;
    protected readonly IBathhouseUnitOfWork _unitOfWork;
    protected readonly IRepository<Client, Guid> _repository;
    protected readonly ILogger<ClientController> _logger;
    protected readonly IMapper _mapper;

    public ClientController(ILogger<ClientController> logger,
                            IMapper mapper,
                            IBathhouseUnitOfWork unitOfWork)
    {
      _officeRepository = unitOfWork.Repository<Office, Guid>();
      _logger = logger;
      _mapper = mapper;
      _unitOfWork = unitOfWork;
      _repository = _unitOfWork.Repository<Client, Guid>();
    }

    #region CRUD endpoints
    /// <summary>
    /// Get all of Clients
    /// </summary>
    [HttpGet(Name = ("GetAll[controller]s"))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<IEnumerable<ClientResponse>> GetAll()
    {
      try
      {
        var allEntities = _repository.GetAll(
          includePropertyNames: new[] { "Office" },
          orderBy: all => all.OrderBy(c => c.LastName));

        _logger.LogInformation($"All of Clients was got.");

        return Ok(_mapper.Map<IEnumerable<Client>, IEnumerable<ClientResponse>>(allEntities));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting all of Clients an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting all of Clients an exception was fired.");
      }
    }

    /// <summary>
    /// Get Client by ID
    /// </summary>
    /// <param name="clientId">The Client ID</param>
    [HttpGet("{clientId:guid}", Name = ("Get[controller]ById"))]
    public ActionResult<ClientResponse> GetById(Guid clientId)
    {
      try
      {
        Client? entity = _repository.Get(key: clientId, includePropertyNames: new[] { "Office" });

        if (entity is null)
        {
          _logger.LogInformation($"Request on getting unexisting Client id={clientId} was received.");
          return NotFound($"Client with ID={clientId} was not found.");
        }

        _logger.LogInformation($"Client id={clientId} was getting successfully.");
        return Ok(_mapper.Map<Client, ClientResponse>(entity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Client id={clientId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Client id={clientId} an exception was fired");
      }
    }

    /// <summary>
    /// Add Client.
    /// </summary>
    /// <param name="request">Newly creating Client</param>
    [HttpPost(Name = ("Create[controller]"))]
    public ActionResult<ClientResponse> Create(ClientRequest request)
    {
      try
      {
        if (request.OfficeId is not null && !_officeRepository.Exist(request.OfficeId.Value))
        {
          _logger.LogInformation($"Office with ID={request.OfficeId} was not found.");
          return NotFound($"Office with ID={request.OfficeId} was not found.");
        }

        Client newEntity = _repository.Add(_mapper.Map<ClientRequest, Client>(request));

        _unitOfWork.Complete();
        _logger.LogInformation($"Client id={newEntity.Id} was creating successfully.");

        return CreatedAtAction("GetById", new { id = newEntity.Id }, _mapper.Map<Client, ClientResponse>(newEntity));
      }
      catch (Exception ex)
      {
        _logger.LogError($"While creating Client an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While creating Client an exception was fired");
      }
    }

    /// <summary>
    /// Update Client
    /// </summary>
    /// <param name="request">Client for updating</param>
    /// <param name="clientId">ID of Client for updating</param>
    /// <returns></returns>
    [HttpPut("{clientId:guid}", Name = ("Update[controller]"))]
    public ActionResult Update(Guid clientId, ClientRequest request)
    {
      try
      {
        if (request.OfficeId is not null && !_officeRepository.Exist(request.OfficeId.Value))
        {
          _logger.LogInformation($"Office with ID={request.OfficeId} was not found.");
          return NotFound($"Office with ID={request.OfficeId} was not found.");
        }

        Client? entity = _repository.Get(clientId);
        if (entity is null)
        {
          _logger.LogInformation($"Client with ID={clientId} was not found.");
          return NotFound($"Client with ID={clientId} was not found.");
        }

        Client updatedEntity = _mapper.Map<ClientRequest, Client>(request, entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Client id={clientId} was updated successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While updating Client an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While updating Client an exception was fired");
      }
    }

    /// <summary>
    /// Delete Client by ID
    /// </summary>
    /// <param name="clientId">Client ID</param>
    [HttpDelete("{clientId:guid}", Name = ("Delete[controller]"))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid clientId)
    {
      try
      {
        Client? entity = _repository.Get(clientId);
        if (entity is null)
        {
          _logger.LogInformation($"Client with ID={clientId} was not found.");
          return NotFound($"Client with ID={clientId} was not found.");
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Client id={clientId} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Client id={clientId} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Client id={clientId} an exception was fired");
      }
    }
    #endregion
  }
}
