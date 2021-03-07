using AutoMapper;
using Bathhouse.Contracts;
using Bathhouse.Contracts.Models.Requests.v1;
using Bathhouse.Contracts.Models.Responses.v1;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.AspNetCore;
using Chuk.Helpers.AspNetCore.ApiConvension;
using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Authorization;
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
    [HttpGet(ApiRoutes.GetAllClients, Name = nameof(ApiRoutes.GetAllClients))]
    [ApiConventionMethod(typeof(DefaultGetAllApiConvension), nameof(DefaultGetAllApiConvension.GetAll))]
    public ActionResult<PaginatedResponse<ClientResponse>> GetAll(
      [FromQuery] PaginationQuery paginationQuery, 
      [FromQuery] Guid? officeId)
    {
      PaginationFilter paginationFilter = new() {
        PageSize = paginationQuery.PageSize,
        PageNumber = paginationQuery.PageNumber };

      var allEntities = _repository.GetAll(
        paginationFilter: paginationFilter,
        filter: officeId == null ? null : client => client.OfficeId == officeId,
        includePropertyNames: new[] { "Office" },
        orderBy: all => all.OrderBy(c => c.LastName));

      _logger.LogInformation($"All of Clients was got.");

      return Ok(new PaginatedResponse<ClientResponse>()
      {
        Data = _mapper.Map<IEnumerable<Client>, IEnumerable<ClientResponse>>(allEntities),
        PageNumber = paginationFilter.IsValid ? paginationFilter.PageNumber : null,
        PageSize = paginationFilter.IsValid ? paginationFilter.PageSize : null
      });
    }

    /// <summary>
    /// Get Client by ID
    /// </summary>
    /// <param name="clientId">The Client ID</param>
    [HttpGet(ApiRoutes.GetClientById, Name = nameof(ApiRoutes.GetClientById))]
    public ActionResult<ClientResponse> GetById(Guid clientId)
    {
      Client? entity = _repository.Get(key: clientId, includePropertyNames: new[] { "Office" });

      if (entity is null)
      {
        _logger.LogInformation($"Client with ID={clientId} was not found.");
        return NotFound($"Client with ID={clientId} was not found.");
      }

      _logger.LogInformation($"Client id={clientId} was getting successfully.");
      return Ok(_mapper.Map<Client, ClientResponse>(entity));
    }

    /// <summary>
    /// Add Client.
    /// </summary>
    /// <param name="request">Newly creating Client</param>
    [HttpPost(ApiRoutes.CreateClient, Name = nameof(ApiRoutes.CreateClient))]
    public ActionResult<ClientResponse> Create(ClientRequest request)
    {
      if (request.OfficeId is not null && !_officeRepository.Exist(request.OfficeId.Value))
      {
        _logger.LogInformation($"Office with ID={request.OfficeId} was not found.");
        return NotFound($"Office with ID={request.OfficeId} was not found.");
      }

      Client newEntity = _repository.Add(_mapper.Map<ClientRequest, Client>(request));

      _unitOfWork.Complete();
      _logger.LogInformation($"Client id={newEntity.Id} was creating successfully.");

      return CreatedAtAction(
        nameof(GetById), 
        new { clientId = newEntity.Id }, 
        _mapper.Map<Client, ClientResponse>(newEntity));
    }

    /// <summary>
    /// Update Client
    /// </summary>
    /// <param name="request">Client for updating</param>
    /// <param name="clientId">ID of Client for updating</param>
    [HttpPut(ApiRoutes.UpdateClient, Name = nameof(ApiRoutes.UpdateClient))]
    public ActionResult Update(Guid clientId, ClientRequest request)
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

      _mapper.Map<ClientRequest, Client>(request, entity);

      _unitOfWork.Complete();
      _logger.LogInformation($"Client id={clientId} was updated successfully.");

      return NoContent();
    }

    /// <summary>
    /// Delete Client by ID
    /// </summary>
    /// <param name="clientId">Client ID</param>
    [HttpDelete(ApiRoutes.DeleteClient, Name = nameof(ApiRoutes.DeleteClient))]
    [ApiConventionMethod(typeof(DefaultDeleteApiConvension), nameof(DefaultDeleteApiConvension.Delete))]
    public IActionResult Delete(Guid clientId)
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
    #endregion
  }
}
