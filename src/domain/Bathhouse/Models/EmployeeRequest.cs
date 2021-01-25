﻿using Bathhouse.ValueTypes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bathhouse.Models
{
  public class EmployeeRequest
  {
    /// <summary>
    /// LastName of employee
    /// </summary>
    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Фамилия")]
    public string LastName { get; set; } = "Фамилия";

    /// <summary>
    /// FirstName of employee
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Имя")]
    public string FirstName { get; set; } = "Имя";

    /// <summary>
    /// Middle Name of employee
    /// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    [DefaultValue("Отчество")]
    public string MiddleName { get; set; } = "Отчество";

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
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date, ErrorMessage = "Incorrect date format.")]
    [DefaultValue("1950-01-01")]
    public DateTime? DoB { get; set; } = DateTime.Parse("1950-01-01");

    /// <summary>
    /// Type of employee
    /// </summary>
    [DefaultValue(EmployeeType.Manager)]
    public EmployeeType Type { get; set; } = EmployeeType.Manager;
  }
}
