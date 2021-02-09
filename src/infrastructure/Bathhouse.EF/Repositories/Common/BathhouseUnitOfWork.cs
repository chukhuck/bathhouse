using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Bathhouse.Repositories.Common;
using chukhuck.Helpers.Patterns;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bathhouse.EF.Repositories.Common
{
  public class BathhouseUnitOfWork : EFUnitOfWork<BathhouseContext>, IBathhouseUnitOfWork
  {
    public BathhouseUnitOfWork(BathhouseContext context) : base(context)
    {
      Answers = new AnswerRepository(context);
      Clients = new ClientRepository(context);
      Employees = new EmployeeRepository(context);
      Offices = new OfficeRepository(context);
      Questions = new QuestionRepository(context);
      Surveys = new SurveyRepository(context);
      SurveyResults = new SurveyResultRepository(context);
      //Roles = new RoleRepository(context);
      WorkItems = new WorkItemRepository(context);
    }

    public IAnswerRepository Answers { get; }

    public IClientRepository Clients { get; }

    public IEmployeeRepository Employees { get; }

    public IOfficeRepository Offices { get; }

    public IQuestionRepository Questions { get; }

    public ISurveyRepository Surveys { get; }

    public ISurveyResultRepository SurveyResults { get; }

    //public IRoleRepository Roles { get; }

    public IWorkItemRepository WorkItems { get; }

    public override IRepository<TEntity, TKeyEntity> Repository<TEntity, TKeyEntity>() 
    {
      TEntity entity = new TEntity();

      return entity switch
      {
        Answer _ => (IRepository<TEntity, TKeyEntity>)Answers,
        Client _ => (IRepository<TEntity, TKeyEntity>)Clients,
        Employee _ => (IRepository<TEntity, TKeyEntity>)Employees,
        Office _ => (IRepository<TEntity, TKeyEntity>)Offices,
        Question _ => (IRepository<TEntity, TKeyEntity>)Questions,
        //IdentityRole _ => (IRepository<TEntity, TKeyEntity>)Roles,
        Survey _ => (IRepository<TEntity, TKeyEntity>)Surveys,
        SurveyResult _ => (IRepository<TEntity, TKeyEntity>)SurveyResults,
        WorkItem _ => (IRepository<TEntity, TKeyEntity>)WorkItems,
        _ => throw new ArgumentNullException(nameof(TEntity), $"Unrecognised type {typeof(TEntity)}.")
      };
    }
  }
}
