using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public record OfficeResponse(
    Guid Id,
    int Number,
    string Address,
    string Phone,
    string Email,
    DateTime TimeOfOpen,
    DateTime TimeOfClose,
    string WorkingTimeRange,
    string ManagerFullName,
    Guid ManagerId
    );
}
