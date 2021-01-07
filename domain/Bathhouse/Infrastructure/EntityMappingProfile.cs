﻿using AutoMapper;
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
      CreateMap<Client, ClientModel>()
      //  .ForMember(dest => dest.OfficeNumber, act => act.MapFrom(src => src.Office.Number))
        .ReverseMap();
      CreateMap<WorkItem, WorkItemModel>()
        .ForMember(dest => dest.CreatorShortName, act => act.MapFrom(src => src.Creator.LastName + " " + src.Creator.FirstName.First() + "."))
        .ForMember(dest => dest.ExecutorShortName, act => act.MapFrom(src => src.Executor.LastName + " " + src.Executor.FirstName.First() + "."))
        .ReverseMap();
      CreateMap<Survey, SurveyModel>().ReverseMap();

      CreateMap<Question, QuestionModel>().ReverseMap();
      //CreateMap<Answer, AnswerModel>()
      //  .ForMember(dest => dest.EmployeeName, act => act.MapFrom(src => src.Author.LastName + " " + src.Author.FirstName.First() + "."))
      //  .ForMember(dest => dest.EmployeeOffice, act => act.MapFrom(src => src.Author.Office.Number))
      //  .ReverseMap();

      //CreateMap<Survey, Models.SurveyResultModel>()
      //  .ForMember(dest => dest.Answers, act => act.MapFrom(src => src.Questions.SelectMany(q => q.Answers)))
      //  .ReverseMap();
    }
  }
}
