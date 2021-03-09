﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Contracts.Models
{
  public class SurveyRequest
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 50 symbols.")]
    [DefaultValue("Новый опрос")]
    public string Name { get; set; } = "Новый опрос";

    [DataType(DataType.Text)]
    [MaxLength(300, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 300 symbols.")]
    [DefaultValue("Описание нового опроса")]
    public string? Description { get; set; } = "Описание нового опроса";

    //[DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    //public DateTime CreationDate { get; set; } = DateTime.Now;

    [Required]
    public Guid AuthorId { get; set; }

    public ICollection<QuestionRequest> Questions { get; set; } = new List<QuestionRequest>();
  }
}
