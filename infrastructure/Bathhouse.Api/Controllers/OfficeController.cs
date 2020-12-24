using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bathhouse.Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class OfficeController : ControllerBase
  {
    private IOfficeRepository _officeRepository;

    private readonly ILogger<OfficeController> _logger;

    public OfficeController(ILogger<OfficeController> logger, IOfficeRepository office)
    {
      _logger = logger;
      _officeRepository = office;
    }

    [HttpGet]
    public IEnumerable<Office> Get()
    {
      throw new NotImplementedException();
    }
  }
}
