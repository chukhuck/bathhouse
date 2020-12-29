using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Infrastructure
{
  class EntityMappingProfile : Profile
  {
    public EntityMappingProfile()
    {
      CreateMap<Office, OfficeModel>().ReverseMap();
    }
  }
}
