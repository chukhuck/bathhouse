using Bathhouse.Models;
using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
  public class Survey : Entity
  {
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    public string Name { get; set; } = "New survey";
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public SurveyStatus Status { get; set; } = SurveyStatus.Work;


    public Employee Author { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public ICollection<Question> Questions { get; set; } = null!;
    public ICollection<SurveyResult> Results { get; set; } = null!;

    /// <summary>
    /// Get All of results of this survey
    /// </summary>
    /// <returns>Result</returns>
    public BaseSurveySummary GetSummary(SurveyResultSummaryType typeSummary)
    {
      return typeSummary switch
      {
        SurveyResultSummaryType.Base => BaseSurveySummary.Create(this),
        _ => throw new ArgumentException("Type of summary is not defined.")
      };
    } 
  }

  public enum SurveyResultSummaryType
  {
    Base
  }

  public enum SurveyStatus
  {
    Work,
    Archive,
    Deleted
  }
}
