using System;

namespace Bathhouse.Contracts.v1.Models
{
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
