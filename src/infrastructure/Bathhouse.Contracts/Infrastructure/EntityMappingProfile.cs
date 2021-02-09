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
      CreateMap<OfficeRequest, Office>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Employees, opt => opt.Ignore())
        .ForMember(dest => dest.Clients, opt => opt.Ignore());


      CreateMap<Employee, EmployeeResponse>();
      CreateMap<EmployeeRequest, Employee>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Offices, opt => opt.Ignore())
        .ForMember(dest => dest.WorkItems, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedWorkItems, opt => opt.Ignore())
        .ForMember(dest => dest.SurveyResults, opt => opt.Ignore())
        .ForMember(dest => dest.Surveys, opt => opt.Ignore())
        .ForMember(dest => dest.UserName, opt => opt.Ignore())
        .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
        .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
        .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
        .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
        .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
        .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
        .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
        .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
        .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
        .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());


      CreateMap<Client, ClientResponse>();
      CreateMap<ClientRequest, Client> ()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Office, opt => opt.Ignore());


      CreateMap<WorkItem, WorkItemResponse>();
      CreateMap<WorkItemRequest, WorkItem>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Creator, opt => opt.Ignore())
        .ForMember(dest => dest.Executor, opt => opt.Ignore())
        ;

      CreateMap<Survey, SurveyResponse>();
      CreateMap<SurveyRequest, Survey>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
        .ForMember(dest => dest.Status, opt => opt.Ignore())
        .ForMember(dest => dest.Results, opt => opt.Ignore())
        .ForMember(dest => dest.Author, opt => opt.Ignore());


      CreateMap<Question, QuestionResponse>();
      CreateMap<QuestionRequest, Question>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Survey, opt => opt.Ignore())
        .ForMember(dest => dest.SurveyId, opt => opt.Ignore())
        .ForMember(dest => dest.Answers, opt => opt.Ignore());

      CreateMap<SurveySummary, SurveySummaryResponse>();
    }
  }
}
