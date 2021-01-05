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
      CreateMap<Employee, EmployeeModel>().ReverseMap();
      CreateMap<Client, ClientModel>().ReverseMap();
      CreateMap<WorkItem, WorkItemModel>().ReverseMap();
      CreateMap<Survey, SurveyModel>().ReverseMap();

      CreateMap<Question, QuestionModel>().ReverseMap();
      CreateMap<Answer, AnswerModel>()
        .ForMember(dest => dest.EmployeeName, act => act.MapFrom(src => src.Author.LastName + " " + src.Author.FirstName.First() + "."))
        .ForMember(dest => dest.EmployeeOffice, act => act.MapFrom(src => src.Author.Office.Number))
        .ReverseMap();

      CreateMap<Survey, SurveyResultModel>()
        .ForMember(dest => dest.Answers, act => act.MapFrom(src => src.Questions.SelectMany(q => q.Answers)))
        .ReverseMap();
    }
  }
}
