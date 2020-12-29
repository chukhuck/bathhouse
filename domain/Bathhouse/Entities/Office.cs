using System;

namespace Bathhouse.Entities
{
  public class Office : Entity
  {
    public int Number { get; set; } = 0;
    public string Address { get; set; } = "Москва, ";
    public string Phone { get; set; } = "+7-495-XXX-XX-XX";
    public DateTime TimeOfOpen { get; set; } = DateTime.MinValue.AddHours(8).AddMinutes(0);
    public DateTime TimeOfClose { get; set; } = DateTime.MinValue.AddHours(22).AddMinutes(0);
  }
}
