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
    [Required]
    public int Number { get; set; }

    /// <summary>
    /// Address of office
    /// </summary>
    [DefaultValue("Москва, ")]
    public string Address { get; set; } = "Москва, ";

    /// <summary>
    /// Phone of office
    /// </summary>
    [DefaultValue("+7-495-XXX-XX-XX")]
    public string Phone { get; set; } = "+7-495-XXX-XX-XX";

    /// <summary>
    /// Time when office will be opened
    /// </summary>
    //[DefaultValue("")]
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(Hour_Of_Openning);

    /// <summary>
    /// Time when office will be closed
    /// </summary>
    //[DefaultValue("")]
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(Hour_Of_Closing);

    /// <summary>
    /// Working time of office
    /// </summary>
    public string WorkingTimeRange => $"{TimeOfOpen.ToShortTimeString()} - {TimeOfClose.ToShortTimeString()}";
  }
}
