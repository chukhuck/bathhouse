﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
  public class Employee : Entity
  {
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string LastName { get; set; } = "Фамилия";

    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string FirstName { get; set; } = "Имя";

    [Phone(ErrorMessage = "Incorrect phone format.")]
    public string Phone { get; set; } = "+7-495-000-00-00";

    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    public DateTime DoB { get; set; } = DateTime.Parse("1950-01-01");

    public EmployeeType Type { get; set; } = EmployeeType.Manager;

    public ICollection<Answer> Answers { get; set; }

    public Guid OfficeId { get; set; }

    public Office Office { get; set; }
  }


  public enum EmployeeType
  {
    Director,
    Manager,
    Employee,
    TechnicalSupport
  }
}
