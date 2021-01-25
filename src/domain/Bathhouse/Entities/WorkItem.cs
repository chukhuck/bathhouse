using Bathhouse.ValueTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Entities
{
  public class WorkItem : Entity
  {
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Description { get; set; } = String.Empty;
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Created;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
    public bool IsImportant { get; set; } = false;


    public Employee Creator { get; set; } = null!;
    public Guid CreatorId { get; set; }
    public Employee Executor { get; set; } = null!;
    public Guid ExecutorId { get; set; }
  }
}
