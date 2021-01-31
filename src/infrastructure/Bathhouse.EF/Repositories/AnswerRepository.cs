using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class AnswerRepository : Repository<Answer>, IAnswerRepository
  {
    public AnswerRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
