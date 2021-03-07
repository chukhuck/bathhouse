using System;

namespace Bathhouse.Api.Contracts.Models.Responses.v1
{
  public record RoleResponse(
  Guid Id,
  string Name,
  string? NormalizedName);
}
