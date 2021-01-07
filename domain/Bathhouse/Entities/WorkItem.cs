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
    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Description { get; set; } = "Опиши текст задачи.";

    public WorkItemStatus Status { get; set; } = WorkItemStatus.Created;

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime EndDate { get; set; } = DateTime.Now;

    public bool IsImportant { get; set; } = false;
  }

  public enum WorkItemStatus
  {
    Created,
    InWork,
    Done,
    Canceled
  }
}
