using Bathhouse.Contracts.v1.Models.Queries;
using Bathhouse.Entities;
using Chuk.Helpers.Expressions;
using System;
using System.Linq.Expressions;

namespace Bathhouse.Api.Common.Filters
{
  public class SurveyFilter
  {
    private readonly SurveyFilterQuery query;
    public SurveyFilter(SurveyFilterQuery query)
    {
      this.query = query;
    }

    public Expression<Func<Survey, bool>> Compose()
    {
      Expression<Func<Survey, bool>> filterByAuthor = query?.AuthorId == null ? wi => true : wi => wi.AuthorId == query.AuthorId;
      Expression<Func<Survey, bool>> filterByStatus = query?.Status == null ? wi => true : wi => wi.Status == query.Status;

      return filterByAuthor.And(filterByStatus);
    }
  }
}
