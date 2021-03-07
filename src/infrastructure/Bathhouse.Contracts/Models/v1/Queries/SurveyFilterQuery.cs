using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.v1.Models.Queries
{
  public class SurveyFilterQuery
  {
    public Guid? AuthorId { get; set; }
    public SurveyStatus? Status { get; set; }
  }
}
