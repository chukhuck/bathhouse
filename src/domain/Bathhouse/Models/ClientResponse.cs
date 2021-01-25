﻿using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Models
{
  public record ClientResponse(
    Guid Id, 
    string LastName, 
    string MiddleName, 
    string FirstName, 
    string? Phone, 
    DateTime? DoB, 
    string? Comment,
    Sex Sex,
    string OfficeNumber,
    Guid OfficeId);
}
