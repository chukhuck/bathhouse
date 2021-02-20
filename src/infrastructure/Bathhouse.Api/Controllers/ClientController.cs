using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
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
    /// <response code="200">Getting all of Clients was successful</response>
    /// <response code="500">Exception on server side was fired</response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<IEnumerable<ClientResponse>> Get()
    {
      try
      {
        var allEntities = _repository.GetAll(
          includePropertyNames: new[] { "Office" }, 
          orderBy: all=> all.OrderBy(c=>c.LastName));

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
    /// <param name="id">The Client ID</param>
    /// <response code="404">Client with current ID is not found</response>
    /// <response code="200">Getting Client is successul</response>
    /// <response code="400">If the request is null</response>
    /// <response code="500">Exception on server side was fired</response>
    [HttpGet()]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<ClientResponse> GetById(Guid id)
    {
      try
      {
        if (_repository.Get(key: id, includePropertyNames: new[] { "Office"}) is Client entity)
        {
          _logger.LogInformation($"Client id={id} was getting successfully.");
          return Ok(_mapper.Map<Client, ClientResponse>(entity));
        }
        else
        {
          _logger.LogInformation($"Request on getting unexisting Client id={id} was received.");
          return NotFound($"Client with ID={id} was not found.");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"While getting Client id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While getting Client id={id} an exception was fired");
      }
    }

    /// <summary>
    /// Add Client.
    /// </summary>
    /// <param name="request">Newly creating Client</param>
    /// <response code="201">Creating Client is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the request is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult<ClientResponse> Create(ClientRequest request)
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
    /// <param name="id">ID of Client for updating</param>
    /// <response code="204">Updating Client is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Client with current ID is not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public virtual ActionResult Update(Guid id, ClientRequest request)
    {
      try
      {
        if (request.OfficeId is not null && !_officeRepository.Exist(request.OfficeId.Value))
        {
          _logger.LogInformation($"Office with ID={request.OfficeId} was not found.");
          return NotFound($"Office with ID={request.OfficeId} was not found.");
        }

        if (_repository.Get(id) is Client entity)
        {
          Client updatedEntity = _mapper.Map<ClientRequest, Client>(request, entity);

          _unitOfWork.Complete();
          _logger.LogInformation($"Client id={id} was updated successfully.");

          return NoContent();
        }
        else
        {
          _logger.LogInformation($"Client with ID={id} was not found.");
          return NotFound($"Client with ID={id} was not found.");
        }
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
    /// <param name="id">Client ID</param>
    /// <response code="404">Client with current ID is not found</response>
    /// <response code="204">Deleting Client is successul</response>
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
        Client? entity = _repository.Get(id);
        if (entity is null)
        {
          _logger.LogInformation($"Client with ID={id} was not found.");
          return NotFound($"Client with ID={id} was not found.");
        }

        _repository.Delete(entity);

        _unitOfWork.Complete();
        _logger.LogInformation($"Client id={id} was deleted successfully.");

        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError($"While deleting Client id={id} an exception was fired. Exception: {ex.Data}. Inner ex: {ex.InnerException}");
        return StatusCode(StatusCodes.Status500InternalServerError, $"While deleting Client id={id} an exception was fired");
      }
    }
    #endregion
  }
}
