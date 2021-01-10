using Bathhouse.Models;
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
    [Required]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    public string Name { get; set; }
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public SurveyStatus Status { get; set; }



    public ICollection<Question> Questions { get; set; }
    public ICollection<SurveyResult> Results { get; set; }

    /// <summary>
    /// Get All of results of this survey
    /// </summary>
    /// <returns>Result</returns>
    public BaseSurveyResultSummary GetSummary(SurveyResultSummaryType typeSummary)
    {
      return typeSummary switch
      {
        SurveyResultSummaryType.Base => BaseSurveyResultSummary.Create(this),
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
