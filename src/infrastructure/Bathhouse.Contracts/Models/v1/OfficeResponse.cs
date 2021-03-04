using System;

namespace Bathhouse.Contracts.v1.Models
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
