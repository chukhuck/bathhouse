using Bathhouse.ValueTypes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Models
{
  public class WorkItemRequest
  {
    /// <summary>
    /// Description of task.
    /// </summary>
    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Опиши текст задачи.")]
    public string Description { get; set; } = "Опиши текст задачи.";

    /// <summary>
    /// Day of creation work item
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Day of start work item
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Day of end work item
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

    /// <summary>
    /// Status  of work item
    /// </summary>
    [DefaultValue(WorkItemStatus.Created)]
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Created;

    /// <summary>
    /// Flag that indicate importance of workitem
    /// </summary>
    [DefaultValue(false)]
    public bool IsImportant { get; set; } = false;

    /// <summary>
    /// Id of workitem creator
    /// </summary>
    [Required]
    public Guid CreatorId { get; set; }

    /// <summary>
    /// Id of workitem executor
    /// </summary>
    [Required]
    public Guid ExecutorId { get; set; }
  }
}
