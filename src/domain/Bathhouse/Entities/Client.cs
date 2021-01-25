using Bathhouse.ValueTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class Client : Entity
  {
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string LastName { get; set; } = "DefaultLastName";
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string MiddleName { get; set; } = String.Empty;
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string FirstName { get; set; } = String.Empty;
    public string? Phone { get; set; }
    public DateTime? DoB { get; set; }
    [MaxLength(250, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string? Comment { get; set; }
    public Sex Sex { get; set; } = Sex.Unknown;

    public Office Office { get; set; } = null!;
    public Guid OfficeId { get; set; }
  }
}
