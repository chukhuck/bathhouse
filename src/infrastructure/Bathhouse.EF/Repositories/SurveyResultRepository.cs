using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class SurveyResultRepository : Repository<SurveyResult>, ISurveyResultRepository
  {
    public SurveyResultRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
