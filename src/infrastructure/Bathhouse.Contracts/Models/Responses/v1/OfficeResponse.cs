using System;

namespace Bathhouse.Contracts.Models.Responses.v1
{
  public record OfficeResponse(
    Guid Id,
    int Number,
    string? Address,
    string? Phone,
    string? Email,
    DateTime TimeOfOpen,
    DateTime TimeOfClose,
    string WorkingTimeRange
    );
}
