using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bathhouse.Api.Contracts.Test
{
  public class ValidationResultEquilityComparer : IEqualityComparer<ValidationResult>
  {
    public bool Equals(ValidationResult x, ValidationResult y)
    {
      if (x is null && y is null)
        return true;

      if (x is null || y is null)
        return false;

      return x.ErrorMessage == y.ErrorMessage 
        && !x.MemberNames.Except(y.MemberNames).Any() 
        && !y.MemberNames.Except(x.MemberNames).Any();
    }

    public int GetHashCode([DisallowNull] ValidationResult obj)
    {
      if (obj.ErrorMessage is null)
        return 0;

      return obj.ErrorMessage.GetHashCode();
    }
  }
}
