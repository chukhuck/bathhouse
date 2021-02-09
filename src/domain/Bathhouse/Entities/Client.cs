using Bathhouse.ValueTypes;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.Entities
{
  public class Client : IEntity<Guid>
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LastName { get; set; } = "DefaultLastName";
    public string MiddleName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? DoB { get; set; }
    public string? Comment { get; set; }
    public Sex Sex { get; set; } = Sex.Unknown;

    public virtual Office Office { get; set; } = null!;
    public Guid OfficeId { get; set; }
  }
}
