using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class QuestionRepository : EFRepository<Question, Guid>, IQuestionRepository
  {
    public QuestionRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
