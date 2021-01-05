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
  public class WorkItemController : RichControllerBase<WorkItem, WorkItemModel>
  {
    public WorkItemController(ILogger<RichControllerBase<WorkItem, WorkItemModel>> logger, IMapper mapper, ICRUDRepository<WorkItem> repository)
      : base(logger, mapper, repository)
    {
    }


  }
}
