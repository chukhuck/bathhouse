using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class OfficeModel
  {
    public Guid Id { get; set; }

    public int Number { get; set; } = 0;
    public string Address { get; set; } = "Москва, ";
    public string Phone { get; set; } = "+7-495-XXX-XX-XX";
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(8);
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(22);
    public string WorkingTimeRange => $"{TimeOfOpen.ToShortTimeString()} - {TimeOfClose.ToShortTimeString()}";
  }
}
