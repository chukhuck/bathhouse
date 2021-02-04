using Bathhouse.Entities;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;

namespace Bathhouse.Models
{
#nullable enable
  public record EmployeeResponse(
    Guid Id,
    string LastName,
    string FirstName,
    string MiddleName,
    string ShortName,
    string FullName,
    string? PhoneNumber,
    string? Email,
    DateTime? DoB
    );
}
