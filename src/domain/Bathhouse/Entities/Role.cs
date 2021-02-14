using Chuk.Helpers.Patterns;
using Microsoft.AspNetCore.Identity;
using System;

namespace Bathhouse.Entities
{
  public class Role : IdentityRole<Guid>, IEntity<Guid>
  {
  }
}
