using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.v1.Models
{
  public record QuestionResponse(
    Guid Id,
    string Text,
    string Name,
    bool IsKey,
    QuestionType Type);
}