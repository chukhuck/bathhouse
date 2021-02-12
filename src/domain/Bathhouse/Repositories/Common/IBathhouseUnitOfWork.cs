using Chuk.Helpers.Patterns;

namespace Bathhouse.Repositories.Common
{
  public interface IBathhouseUnitOfWork : IUnitOfWork
  {
    IAnswerRepository Answers { get; }
    IClientRepository Clients { get; }
    IEmployeeRepository Employees { get; }
    IOfficeRepository Offices { get; }
    IQuestionRepository Questions { get; }
    ISurveyRepository Surveys { get; }
    ISurveyResultRepository SurveyResults { get; }
    //IRoleRepository Roles { get; }
    IWorkItemRepository WorkItems { get; }
  }
}
