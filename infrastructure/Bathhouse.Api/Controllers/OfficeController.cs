using Bathhouse.Entities;
using Bathhouse.Repositories;
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
      return _officeRepository.GetAll();
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Office GetById(Guid id)
    {
      return _officeRepository.GetById(id);
    }

    [HttpGet]
    [Route("{numberOfOffice:int}")]
    public Office GetByNumber(int numberOfOffice)
    {
      return _officeRepository.GetByNumber(numberOfOffice);
    }

    [HttpPost]
    public IActionResult Add(Office office)
    {
      _officeRepository.Add(office);

      return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(Guid id, Office office)
    {
      _officeRepository.Update(id, office);

      return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
      if (_officeRepository.GetById(id) is not Office)
        return NotFound();

      _officeRepository.Delete(id);

      return Ok();
    }
  }
}
