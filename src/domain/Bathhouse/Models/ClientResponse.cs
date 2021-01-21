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
  public record ClientResponse(
    Guid id, 
    string LastName, 
    string MiddleName, 
    string FirstName, 
    string? Phone, 
    DateTime? DoB, 
    string? Comment,
    Sex Sex,
    string OfficeNumber,
    Guid OfficeId);
}
