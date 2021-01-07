﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bathhouse.Entities
{
  public class SurveyResult : Entity
  {
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    public Employee Author { get; set; }

    public Guid AuthorId { get; set; }

    public Survey Survey { get; set; }

    public Guid SurveyId { get; set; }

    public ICollection<Answer> Answers { get; set; }


    public virtual List<string> ToList()
    {
      List<string> row = new List<string>();

      row.Add(CreationDate.ToString());
      row.Add(Author.LastName);

      row.AddRange(Answers.Select(a => a.Value));

      return row;
    }
  }
}