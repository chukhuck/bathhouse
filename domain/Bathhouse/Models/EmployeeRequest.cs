﻿using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Models
{
  public class EmployeeRequest
  {
    /// <summary>
    /// LastName of employee
    /// </summary>
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Фамилия")]
    public string LastName { get; set; } = "Фамилия";

    /// <summary>
    /// FirstName of employee
    /// </summary>
    [DataType(DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Имя")]
    public string FirstName { get; set; } = "Имя";

    /// <summary>
    /// Phone of employee
    /// </summary>
    [Phone(ErrorMessage = "Incorrect phone format.")]
    [DefaultValue("+7-495-000-00-00")]
    public string? Phone { get; set; } = "+7-495-000-00-00";

    /// <summary>
    /// Email of employee
    /// </summary>
    [DefaultValue("noreply@mail.com")]
    [EmailAddress(ErrorMessage = "Incorrect email address format.")]
    public string? Email { get; set; } = "noreply@mail.com";

    /// <summary>
    /// Day of Birth
    /// </summary>
    [DataType(DataType.Date, ErrorMessage = "Incorrect date format.")]
    [DefaultValue("1950-01-01")]
    public DateTime? DoB { get; set; } = DateTime.Parse("1950-01-01");

    /// <summary>
    /// Type of employee
    /// </summary>
    [DefaultValue(EmployeeType.Manager)]
    public EmployeeType Type { get; set; } = EmployeeType.Manager;
  }
}
