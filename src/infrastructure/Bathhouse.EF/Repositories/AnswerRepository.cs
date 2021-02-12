using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class AnswerRepository : EFRepository<Answer, Guid>, IAnswerRepository
  {
    public AnswerRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
