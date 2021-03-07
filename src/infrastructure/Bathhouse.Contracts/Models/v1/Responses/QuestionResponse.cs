using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.Models.v1.Responses
{
  public record QuestionResponse(
    Guid Id,
    string Text,
    string Name,
    bool IsKey,
    QuestionType Type);
}