﻿using Bathhouse.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    public SurveySummary GetSummary(SurveyResultSummaryType typeSummary)
    {
      return SurveySummary.Create(this, typeSummary);
    } 
  }
}
