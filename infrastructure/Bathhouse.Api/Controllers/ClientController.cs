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
  public class ClientController : RichControllerBase<Client, ClientResponse, ClientRequest>
  {
    protected readonly ICRUDRepository<Office> _officeRepository;

    public ClientController(ILogger<RichControllerBase<Client, ClientResponse, ClientRequest>> logger, 
                            IMapper mapper, 
                            ICRUDRepository<Client> repository,
                            ICRUDRepository<Office> officeRepository)
      : base(logger, mapper, repository)
    {
      _officeRepository = officeRepository;
    }

    /// <summary>
    /// Create and add entity.
    /// </summary>
    /// <param name="request">Newly creating entity</param>
    /// <response code="201">Creating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Office was not found</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
    public override ActionResult<ClientResponse> Create(ClientRequest request)
    {
      if (!_officeRepository.Exist(request.OfficeId))
      {
        _logger.LogInformation($"Office with ID={request.OfficeId} was not found.");
        return NotFound($"Office with ID={request.OfficeId} was not found.");
      }

      return base.Create(request);
    }

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="request">Entity for updating</param>
    /// <param name="id">ID of entity for updating</param>
    /// <response code="201">Updating entity is successul</response>
    /// <response code="500">Exception on server side was fired</response>
    /// <response code="400">If the item is null</response>
    /// <response code="404">Client with current ID is not found or Office with defined ID was not found</response>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType((int)StatusCodes.Status400BadRequest)]
    [ProducesResponseType((int)StatusCodes.Status404NotFound)]
    public override ActionResult Update(Guid id, ClientRequest request)
    {
      if (!_officeRepository.Exist(request.OfficeId))
      {
        _logger.LogInformation($"Office with ID={request.OfficeId} was not found.");
        return NotFound($"Office with ID={request.OfficeId} was not found.");
      }

      return base.Update(id, request);
    }


  }
}
