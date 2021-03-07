using System;

namespace Bathhouse.Contracts.Models.v1.Responses
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
