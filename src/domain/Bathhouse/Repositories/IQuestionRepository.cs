using Bathhouse.Entities;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.Repositories
{
  public interface IQuestionRepository : IRepository<Question, Guid>
  {
  }
}
