using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bathhouse.Api.Controllers
{
  [Route("[controller]")]
  public class WorkItemController : RichControllerBase<WorkItem, WorkItemResponse, WorkItemRequest>
  {
    public WorkItemController(ILogger<RichControllerBase<WorkItem, WorkItemResponse, WorkItemRequest>> logger, IMapper mapper, IUnitOfWork unitOfWork)
      : base(logger, mapper, unitOfWork)
    {
    }


  }
}
