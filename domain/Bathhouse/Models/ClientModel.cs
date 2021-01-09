using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class ClientModel : EntityModel
  {
    /// <summary>
    /// LastName of client
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Фамилия")]
    public string LastName { get; set; } = "Фамилия";

    /// <summary>
    /// MiddleName of client
    /// </summary>
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Имя")]
    public string MiddleName { get; set; } = "Отчество";

    /// <summary>
    /// FirstName of client
    /// </summary>
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Имя")]
    public string FirstName { get; set; } = "Имя";

    /// <summary>
    /// Phone of client
    /// </summary>
    [Phone(ErrorMessage = "Incorrect phone format.")]
    [DefaultValue("+7-495-000-00-00")]
    public string Phone { get; set; } = "+7-495-000-00-00";

    /// <summary>
    /// Day of Birth
    /// </summary>
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    [DefaultValue("1950-01-01")]
    public DateTime DoB { get; set; } = DateTime.Parse("1950-01-01");

    /// <summary>
    /// Comment for client
    /// </summary>
    [DataType(DataType.Text)]
    [MaxLength(250, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Комментарий")]
    public string Comment { get; set; } = "Комментарий";

    /// <summary>
    /// Gender of client
    /// </summary>
    [DefaultValue(Sex.Unknown)]
    public Sex Sex { get; set; } = Sex.Unknown;

    /// <summary>
    /// Number of "home" office for client
    /// </summary>
    public int OfficeNumber { get; set; }

    /// <summary>
    /// ID of "home" office for client
    /// </summary>
    public Guid OfficeId { get; set; }
  }
}
