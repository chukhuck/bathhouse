using System;
using System.Linq;

namespace Bathhouse.Memory
{
  public class OfficeRepository : IOfficeRepository
  {
    private readonly Office[] offices = new Office[]
    {
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000126"), Number = 126 },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000001099"), Number = 1099 },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Number = 5 },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000000035"), Number = 35 },
      new () { Id = Guid.Parse("00000000-0000-0000-0000-000000001276"), Number = 1276 }
    };

    public Office GetById(Guid id) => offices.FirstOrDefault(o=>o.Id == id);

    public Office GetByNumber(int numberOfOffice) => offices.FirstOrDefault(o => o.Number == numberOfOffice);
  }
}