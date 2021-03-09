﻿using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Entities
{
  public class Client
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LastName { get; set; } = "DefaultLastName";
    public string MiddleName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? DoB { get; set; }
    public string? Comment { get; set; }
    public Sex Sex { get; set; } = Sex.Unknown;

    public Office Office { get; set; } = null!;
    public Guid OfficeId { get; set; }
  }
}
