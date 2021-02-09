using Bathhouse.Entities;
using System;

namespace Bathhouse.Repositories
{
  public interface IQuestionRepository : IRepository<Question, Guid>
  {
  }
}
