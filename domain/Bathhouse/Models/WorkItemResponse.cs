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
  public record WorkItemResponse(
      Guid Id,
      string Description,
      DateTime CreationDate,
      DateTime StartDate,
      DateTime EndDate,
      WorkItemStatus Status,
      bool IsImportant,
      string CreatorShortName,
      Guid CreatorId,
      string ExecutorShortName,
      Guid ExecutorId
      );
}
