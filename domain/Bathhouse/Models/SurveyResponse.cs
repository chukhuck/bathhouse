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
  public record SurveyResponse(
  Guid Id,
  string Name,
  string? Description,
  DateTime CreationDate,
  SurveyStatus Status,
  ICollection<QuestionResponse> Questions);
}
