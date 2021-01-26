using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Entities
{
  public class Client
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LastName { get; set; } = "DefaultLastName";
    public string MiddleName { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string? Phone { get; set; }
    public DateTime? DoB { get; set; }
    public string? Comment { get; set; }
    public Sex Sex { get; set; } = Sex.Unknown;

    public Office Office { get; set; } = null!;
    public Guid OfficeId { get; set; }
  }
}
