using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using Bathhouse.Repositories;
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
    public ClientController(ILogger<RichControllerBase<Client, ClientResponse, ClientRequest>> logger, IMapper mapper, ICRUDRepository<Client> repository)
      : base(logger, mapper, repository)
    {
    }


  }
}
