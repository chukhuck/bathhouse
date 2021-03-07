using System;

namespace Bathhouse.Contracts.Models.Responses.v1
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
