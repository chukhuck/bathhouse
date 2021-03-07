using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Api.Contracts.Models.Responses.v1
{
  public record ClientResponse(
    Guid Id, 
    string LastName, 
    string MiddleName, 
    string FirstName, 
    string Phone, 
    DateTime? DoB, 
    string? Comment,
    Sex Sex,
    string OfficeNumber,
    Guid? OfficeId);
}
