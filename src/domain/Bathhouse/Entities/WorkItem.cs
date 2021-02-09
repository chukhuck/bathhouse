using Bathhouse.ValueTypes;
using chukhuck.Helpers.Patterns;
using System;

namespace Bathhouse.Entities
{
  public class WorkItem : IEntity<Guid>
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = String.Empty;
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Created;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
    public bool IsImportant { get; set; } = false;
    public bool IsUrgent => EndDate.Date == DateTime.Now.Date;


    public virtual Employee Creator { get; set; } = null!;
    public Guid CreatorId { get; set; }
    public virtual Employee? Executor { get; set; }
    public Guid? ExecutorId { get; set; }
  }
}
