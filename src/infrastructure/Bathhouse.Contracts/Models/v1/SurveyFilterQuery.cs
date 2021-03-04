using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using Chuk.Helpers.Expressions;
using System;
using System.Linq.Expressions;

namespace Bathhouse.Contracts.v1.Models
{
  public class SurveyFilterQuery
  {
    public Guid? AuthorId { get; set; }
    public SurveyStatus? Status { get; set; }

    public Expression<Func<Survey, bool>> Compose()
    {
      Expression<Func<Survey, bool>> filterByAuthor = AuthorId == null ? wi => true : wi => wi.AuthorId == AuthorId;
      Expression<Func<Survey, bool>> filterByStatus = Status == null ? wi => true : wi => wi.Status == Status;

      return filterByAuthor.And(filterByStatus);
    }
  }
}
