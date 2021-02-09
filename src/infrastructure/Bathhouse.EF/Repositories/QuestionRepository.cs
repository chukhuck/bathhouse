using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class QuestionRepository : Repository<Question, Guid>, IQuestionRepository
  {
    public QuestionRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
