using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;

namespace Bathhouse.Contracts.Models.Responses.v1
{
  public record SurveyResponse(
  Guid Id,
  string Name,
  string? Description,
  DateTime CreationDate,
  SurveyStatus Status,
  string AuthorShortName,
  ICollection<QuestionResponse> Questions);
}
