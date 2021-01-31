using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class QuestionRepository : Repository<Question>, IQuestionRepository
  {
    public QuestionRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
