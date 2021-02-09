using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class AnswerRepository : Repository<Answer, Guid>, IAnswerRepository
  {
    public AnswerRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
