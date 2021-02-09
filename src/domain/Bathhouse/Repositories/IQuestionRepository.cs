using Bathhouse.Entities;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.Repositories
{
  public interface IQuestionRepository : IRepository<Question, Guid>
  {
  }
}
