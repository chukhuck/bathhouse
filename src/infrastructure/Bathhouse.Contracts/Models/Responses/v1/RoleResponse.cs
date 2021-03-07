using System;

namespace Bathhouse.Contracts.Models.Responses.v1
{
  public record RoleResponse(
  Guid Id,
  string Name,
  string? NormalizedName);
}
