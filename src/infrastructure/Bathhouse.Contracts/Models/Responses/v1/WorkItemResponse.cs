using Bathhouse.ValueTypes;
using System;

namespace Bathhouse.Contracts.Models.Responses.v1
{
  public record WorkItemResponse(
      Guid Id,
      string Description,
      DateTime CreationDate,
      DateTime StartDate,
      DateTime EndDate,
      WorkItemStatus Status,
      bool IsImportant,
      bool IsUrgent,
      string CreatorShortName,
      Guid CreatorId,
      string ExecutorShortName,
      Guid ExecutorId
      );
}
