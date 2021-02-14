using Bathhouse.ValueTypes;
using Chuk.Helpers.Patterns;
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

    public virtual Office? Office { get; set; }
    public Guid? OfficeId { get; set; }

    public void SetOffice(Office newOffice)
    {
      if (newOffice is null)
      {
        throw new ArgumentNullException(nameof(newOffice));
      }

      Office = newOffice;
      OfficeId = newOffice.Id;
    }
  }
}
