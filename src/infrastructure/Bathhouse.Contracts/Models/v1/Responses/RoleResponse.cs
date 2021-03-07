using System;

namespace Bathhouse.Contracts.Models.v1.Responses
{
  public record RoleResponse(
  Guid Id,
  string Name,
  string? NormalizedName);
}
