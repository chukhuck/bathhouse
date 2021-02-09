using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using chukhuck.Helpers.Patterns;
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
