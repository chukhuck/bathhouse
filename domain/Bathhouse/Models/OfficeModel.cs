using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  /// <summary>
  /// DTO for offices
  /// </summary>
  public class OfficeModel : EntityModel
  {
    private const int Hour_Of_Openning = 8;

    private const int Hour_Of_Closing = 22;

    /// <summary>
    /// Number of office
    /// </summary>
    [DefaultValue(0)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required for filling.")]
    public int Number { get; set; }

    /// <summary>
    /// Address of office
    /// </summary>
    [DefaultValue("Москва, ")]
    [MaxLength(150, ErrorMessage = "Max lenght of field is 150 symbols.")]
    public string Address { get; set; } = "Москва, ";

    /// <summary>
    /// Phone of office
    /// </summary>
    [DefaultValue("+7-495-000-00-00")]
    [Phone(ErrorMessage = "Incorrect phone format.")]
    public string Phone { get; set; } = "+7-495-000-00-00";

    /// <summary>
    /// Email of office
    /// </summary>
    [DefaultValue("noreply@mail.com")]
    [EmailAddress(ErrorMessage = "Incorrect email address format.")]
    public string Email { get; set; } = "noreply@mail.com";

    /// <summary>
    /// Time when office will be opened
    /// </summary>
    //[DefaultValue("")]
    [DataType(DataType.Time, ErrorMessage = "Incorrect time format.")]
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(Hour_Of_Openning);

    /// <summary>
    /// Time when office will be closed
    /// </summary>
    //[DefaultValue("")]
    [DataType(DataType.Time, ErrorMessage = "Incorrect time format.")]
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(Hour_Of_Closing);

    /// <summary>
    /// Working time of office
    /// </summary>
    public string WorkingTimeRange => $"{TimeOfOpen.ToShortTimeString()} - {TimeOfClose.ToShortTimeString()}";

    /// <summary>
    /// Manager Fullname of office
    /// </summary>
    public string ManagerFullName { get; set; }

    /// <summary>
    /// ID Manager of office
    /// </summary>
    public Guid ManagerId { get; set; }
  }
}
