using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Models
{
  public record QuestionResponse(
    Guid Id,
    string Text,
    string Name,
    bool IsKey,
    QuestionType Type);
}