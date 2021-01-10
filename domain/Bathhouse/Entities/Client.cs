using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
  public class Client : Entity
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string LastName { get; set; }

    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string MiddleName { get; set; }

    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string FirstName { get; set; }

    [Phone(ErrorMessage = "Incorrect phone format.")]
    public string Phone { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime DoB { get; set; }

    [DataType(DataType.Text)]
    [MaxLength(250, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string Comment { get; set; }

    public Sex Sex { get; set; }

    public Office Office { get; set; }

    public Guid OfficeId { get; set; }
  }

  public enum Sex
  {
    Male,
    Female,
    Unknown
  }
}
