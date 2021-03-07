using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.Models.Responses.v1
{
  public record QuestionResponse(
    Guid Id,
    string Text,
    string Name,
    bool IsKey,
    QuestionType Type);
}