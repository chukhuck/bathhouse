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
  public class OfficeController : RichControllerBase<Office, OfficeResponse, OfficeRequest>
  {
    public OfficeController(ILogger<RichControllerBase<Office, OfficeResponse, OfficeRequest>> logger, IMapper mapper, ICRUDRepository<Office> repository) 
      : base(logger, mapper, repository)
    {
    }


  }
}
