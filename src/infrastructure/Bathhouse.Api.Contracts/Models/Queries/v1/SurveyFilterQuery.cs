using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Api.Contracts.Models.Queries.v1
{
  public class SurveyFilterQuery
  {
    public Guid? AuthorId { get; set; }
    public SurveyStatus? Status { get; set; }
  }
}
