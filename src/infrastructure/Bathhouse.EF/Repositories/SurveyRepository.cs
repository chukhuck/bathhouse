using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;

namespace Bathhouse.EF.Repositories
{
  public class SurveyRepository : Repository<Survey>, ISurveyRepository
  {
    public SurveyRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
