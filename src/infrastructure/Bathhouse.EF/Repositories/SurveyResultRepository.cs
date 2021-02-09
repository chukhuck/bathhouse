using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.EF.Repositories
{
  public class SurveyResultRepository : EFRepository<SurveyResult, Guid>, ISurveyResultRepository
  {
    public SurveyResultRepository(BathhouseContext _context) : base(_context)
    {
    }
  }
}
