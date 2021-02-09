using Bathhouse.ValueTypes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Contracts.Models
{
  /// <summary>
  /// 
  /// </summary>
  public class ClientRequest
  {
    /// <summary>
    /// LastName of client
    /// </summary>
    [Required(ErrorMessage = "Field LastName is required.")]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [StringLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Фамилия")]
    public string LastName { get; set; } = "Фамилия";

    /// <summary>
    /// MiddleName of client
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Имя")]
    public string MiddleName { get; set; } = "Отчество";

    /// <summary>
    /// FirstName of client
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
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
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date, ErrorMessage = "Incorrect date format.")]
    [DefaultValue("1950-01-01")]
    public DateTime? DoB { get; set; } = DateTime.Parse("1950-01-01");

    /// <summary>
    /// Comment for client
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(250, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 250 symbols.")]
    [DefaultValue("Комментарий")]
    public string? Comment { get; set; } = "Комментарий";

    /// <summary>
    /// Gender of client
    /// </summary>
    [DefaultValue(Sex.Unknown)]
    [EnumDataType(typeof(Sex), ErrorMessage = "Incorrect a data type.")]
    public Sex Sex { get; set; } = Sex.Unknown;

    /// <summary>
    /// OD of "home" office of client
    /// </summary>
    [Required]
    public Guid OfficeId { get; set; }
  }
}
