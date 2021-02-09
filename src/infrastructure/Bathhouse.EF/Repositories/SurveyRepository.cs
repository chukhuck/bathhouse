using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using System;

namespace Bathhouse.EF.Repositories
{
  public class SurveyRepository : Repository<Survey, Guid>, ISurveyRepository
  {
    public SurveyRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
