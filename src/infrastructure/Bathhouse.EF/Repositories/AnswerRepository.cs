using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using chukhuck.Helpers.Patterns;
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
