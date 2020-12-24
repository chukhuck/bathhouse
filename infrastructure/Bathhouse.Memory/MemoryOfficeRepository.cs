using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Memory
{
  public class MemoryOfficeRepository : IOfficeRepository
  {
    private List<Office> offices = new List<Office>
    {
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Number = 1, Address = "ЮАО, ул. Радиальная, д.1", Phone = "+7-499-000-00-01", TimeOfOpen = DateTime.MinValue.AddHours(8), TimeOfClose = DateTime.MinValue.AddHours(18)},
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Number = 2, Address = "ЮАО, ул. Радиальная, д.2", Phone = "+7-499-000-00-02", TimeOfOpen = DateTime.MinValue.AddHours(0), TimeOfClose = DateTime.MinValue.AddHours(0) },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Number = 3, Address = "ЮАО, ул. Радиальная, д.3", Phone = "+7-499-000-00-03", TimeOfOpen = DateTime.MinValue.AddHours(10) },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Number = 4, Address = "ЮАО, ул. Радиальная, д.4", Phone = "+7-499-000-00-04" },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Number = 5, Address = "ЮАО, ул. Радиальная, д.5" },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Number = 6},
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000007")},
      new () { }
    };

    public void Add(Office office)
    {
      offices.Add(office);
    }

    public void Delete(Guid id)
    {
      if (offices.Find(o => o.Id == id) is Office o)
      {
        offices.Remove(o);
      }
    }

    public IEnumerable<Office> GetAll()
    {
      return offices;
    }

    public Office GetById(Guid id) => offices.FirstOrDefault(o=>o.Id == id);

    public Office GetByNumber(int numberOfOffice) => offices.FirstOrDefault(o => o.Number == numberOfOffice);

    public void Update(Guid id, Office office)
    {
      if (offices.Find(o => o.Id == id) is Office o)
      {
        o.Number = office.Number;
        o.Address = office.Address;
        o.Phone = office.Phone;
        o.TimeOfOpen = office.TimeOfOpen;
        o.TimeOfClose = office.TimeOfClose;
      }
    }
  }
}