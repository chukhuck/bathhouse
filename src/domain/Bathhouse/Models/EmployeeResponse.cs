using Bathhouse.ValueTypes;
using System;

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
    string? Phone,
    string? Email,
    DateTime? DoB,
    EmployeeType Type
    );
}
