using System;
using System.Collections.Generic;

namespace Bathhouse
{
  public interface IOfficeRepository
  {
    IEnumerable<Office> GetAll();

    Office GetById(Guid id);

    Office GetByNumber(int numberOfOffice);
  }
}