using System;

namespace Bathhouse.Models
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
