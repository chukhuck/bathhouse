using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Models
{
  public record AnswerResponse(
    Guid Id, 
    DateTime CreationDate, 
    string Value, 
    string EmployeeName, 
    string EmployeeOffice);

}