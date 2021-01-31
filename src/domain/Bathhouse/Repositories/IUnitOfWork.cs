using System;

namespace Bathhouse.Repositories
{
  public interface IUnitOfWork : IDisposable
  {
    IAnswerRepository Answers { get; }
    IClientRepository Clients { get; }
    IEmployeeRepository Employees { get; }
    IOfficeRepository Offices { get; }
    IQuestionRepository Questions { get; }
    ISurveyRepository Surveys { get; }
    ISurveyResultRepository SurveyResults { get; }
    IRoleRepository Roles { get; }
    IWorkItemRepository WorkItems { get; }

    int Complete();

    IRepository<TEntity> Repository<TEntity>() where TEntity : class, new();
  }
}
