using System;

namespace Bathhouse.Contracts.v1.Models
{
  public record RoleResponse(
  Guid Id,
  string Name,
  string? NormalizedName);
}
