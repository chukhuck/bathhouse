using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
  public class WorkItem : Entity
  {
    [Required]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Description { get; set; }
    public WorkItemStatus Status { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsImportant { get; set; } = false;


    public Employee Creator { get; set; }
    [Required]
    public Guid CreatorId { get; set; }
    public Employee Executor { get; set; }
    [Required]
    public Guid ExecutorId { get; set; }
  }

  public enum WorkItemStatus
  {
    Created,
    InWork,
    Done,
    Canceled
  }
}
