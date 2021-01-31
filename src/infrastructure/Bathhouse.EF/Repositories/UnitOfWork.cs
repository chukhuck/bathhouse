using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Identity;
using System;

namespace Bathhouse.EF.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    public UnitOfWork(BathhouseContext context)
    {
      Context = context;
      Answers = new AnswerRepository(Context);
      Clients = new ClientRepository(Context);
      Employees = new EmployeeRepository(Context);
      Offices = new OfficeRepository(Context);
      Questions = new QuestionRepository(Context);
      Surveys = new SurveyRepository(Context);
      SurveyResults = new SurveyResultRepository(Context);
      Roles = new RoleRepository(Context);
      WorkItems = new WorkItemRepository(Context);
    }

    public BathhouseContext Context { get; }

    public IAnswerRepository Answers { get; }

    public IClientRepository Clients { get; }

    public IEmployeeRepository Employees { get; }

    public IOfficeRepository Offices { get; }

    public IQuestionRepository Questions { get; }

    public ISurveyRepository Surveys { get; }

    public ISurveyResultRepository SurveyResults { get; }

    public IRoleRepository Roles { get; }

    public IWorkItemRepository WorkItems { get; }

    public int Complete()
    {
      return Context.SaveChanges();
    }

    public void Dispose()
    {
      Context.Dispose();
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
    {
      TEntity entity = new TEntity();

      return entity switch
      {
        Answer _ => (IRepository<TEntity>)Answers,
        Client _ => (IRepository<TEntity>)Clients,
        Employee _ => (IRepository<TEntity>)Employees,
        Office _ => (IRepository<TEntity>)Offices,
        Question _ => (IRepository<TEntity>)Questions,
        IdentityRole _ => (IRepository<TEntity>)Roles,
        Survey _ => (IRepository<TEntity>)Surveys,
        SurveyResult _ => (IRepository<TEntity>)SurveyResults,
        WorkItem _ => (IRepository<TEntity>)WorkItems,
        _ => throw new ArgumentNullException(nameof(TEntity), $"Unrecognised type {typeof(TEntity)}.")
      };
    }
  }
}
