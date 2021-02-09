using AutoMapper;
using Bathhouse.Contracts.Models;
using Bathhouse.Entities;
using Bathhouse.ValueTypes;

namespace Bathhouse.Contracts.Infrastructure
{
  public class EntityMappingProfile : Profile
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

      CreateMap<SurveySummary, SurveySummaryResponse>();
    }
  }
}
