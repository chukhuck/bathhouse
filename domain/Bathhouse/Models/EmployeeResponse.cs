using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
#nullable enable
  public record EmployeeResponse(
    Guid Id,
    string LastName,
    string FirstName,
    string ShortName,
    string FullName,
    string? Phone,
    string? Email,
    DateTime? DoB,
    EmployeeType Type
    );
}
