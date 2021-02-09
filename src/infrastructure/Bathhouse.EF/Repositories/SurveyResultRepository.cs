using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class SurveyResultRepository : Repository<SurveyResult, Guid>, ISurveyResultRepository
  {
    public SurveyResultRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
