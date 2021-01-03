﻿using AutoMapper;
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
  public class EmployeeController : RichControllerBase<Employee, EmployeeModel>
  {
    public EmployeeController(ILogger<RichControllerBase<Employee, EmployeeModel>> logger, IMapper mapper, ICRUDRepository<Employee> repository)
      : base(logger, mapper, repository)
    {
    }


  }
}
