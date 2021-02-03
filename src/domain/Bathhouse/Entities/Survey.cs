using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;

namespace Bathhouse.Entities
{
  public class Survey
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "New survey";
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public SurveyStatus Status { get; set; } = SurveyStatus.Work;


    public virtual Employee Author { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public virtual ICollection<Question> Questions { get; set; } = null!;
    public virtual ICollection<SurveyResult> Results { get; set; } = null!;

    /// <summary>
    /// Get All of results of this survey
    /// </summary>
    /// <returns>Result</returns>
    public SurveySummary GetSummary(SurveyResultSummaryType typeSummary)
    {
      return SurveySummary.Create(this, typeSummary);
    } 
  }
}
