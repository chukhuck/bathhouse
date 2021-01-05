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
  public class WorkItemModel : EntityModel
  {
    /// <summary>
    /// Description of task.
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Опиши текст задачи.")]
    public string Description { get; set; } = "Опиши текст задачи.";

    /// <summary>
    /// Day of creation work item
    /// </summary>
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Day of start work item
    /// </summary>
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Day of end work item
    /// </summary>
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime EndDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Status  of work item
    /// </summary>
    [DefaultValue(WorkItemStatus.Created)]
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Created;
  }
}
