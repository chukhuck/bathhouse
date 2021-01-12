using AutoMapper;
using Bathhouse.Entities;
using Bathhouse.Models;
using Bathhouse.ValueTypes;
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
      CreateMap<Office, OfficeResponse>();
      CreateMap<OfficeRequest, Office>();


      CreateMap<Employee, EmployeeResponse>();
      CreateMap<EmployeeRequest, Employee > ();


      CreateMap<Client, ClientResponse>();
      CreateMap<ClientRequest, Client> ();


      CreateMap<WorkItem, WorkItemResponse>();
      CreateMap<WorkItemRequest, WorkItem>();

      CreateMap<Survey, SurveyResponse>();
      CreateMap<SurveyRequest, Survey > ();


      CreateMap<Question, QuestionResponse>();
      CreateMap<QuestionRequest, Question > ();

      CreateMap<BaseSurveySummary, BaseSurveySummaryResponse>();
    }
  }
}
