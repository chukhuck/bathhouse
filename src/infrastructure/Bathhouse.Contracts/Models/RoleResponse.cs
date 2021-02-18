using System;

namespace Bathhouse.Contracts.Models
{
  public record RoleResponse(
  Guid Id,
  string Name,
  string? NormalizedName);
}
