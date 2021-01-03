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
  [ApiController]
  [Route("[controller]")]
  public class OfficeController : ControllerBase
  {
    private IOfficeRepository _officeRepository;

    private readonly ILogger<OfficeController> _logger;

    private readonly IMapper _mapper;

    public OfficeController(ILogger<OfficeController> logger, IMapper mapper, IOfficeRepository officeRepository)
    {
      _logger = logger;
      _officeRepository = officeRepository;
      _mapper = mapper;
    }

    [HttpGet]
    public IEnumerable<OfficeModel> Get()
    {
      return _mapper.Map<IEnumerable<Office>, IEnumerable<OfficeModel>>(_officeRepository.GetAll());
    }

    [HttpGet]
    [Route("{id:guid}")]
    public OfficeModel GetById(Guid id)
    {
      return _mapper.Map<Office, OfficeModel>(_officeRepository.GetById(id));
    }

    [HttpGet]
    [Route("{numberOfOffice:int}")]
    public OfficeModel GetByNumber(int numberOfOffice)
    {
      return _mapper.Map<Office, OfficeModel>(_officeRepository.GetByNumber(numberOfOffice));
    }

    [HttpPost]
    public IActionResult Add(OfficeModel office)
    {
      _officeRepository.Add(_mapper.Map<OfficeModel, Office>(office));

      return Ok();
    }

    [HttpPut]
    public IActionResult Update(Guid id, OfficeModel office)
    {
      _officeRepository.Update(id, _mapper.Map<OfficeModel, Office>(office));

      return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
      if (_officeRepository.GetById(id) is not Office)
        return NotFound();

      _officeRepository.Delete(id);

      return Ok();
    }
  }
}
