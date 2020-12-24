using System;

namespace Bathhouse
{
  public class Office : Entity
  {
    public int Number => 0;
    public string Adress => "Москва, ";
    public string Phone => "+7-495-XXX-XX-XX";
    public DateTime TimeOfOpen => DateTime.MinValue.AddHours(8).AddMinutes(0);
    public DateTimeOffset TimeOfClose => DateTime.MinValue.AddHours(22).AddMinutes(0);
  }
}
