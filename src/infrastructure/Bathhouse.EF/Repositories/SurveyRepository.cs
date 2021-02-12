using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Chuk.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class SurveyRepository : EFRepository<Survey, Guid>, ISurveyRepository
  {
    public SurveyRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
