using System;
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
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string LastName { get; set; }
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string FirstName { get; set; }
    public string FullName => LastName + " " + FirstName;
    public string ShortName => LastName + " " + FirstName.FirstOrDefault() + ".";
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime DoB { get; set; }
    public EmployeeType Type { get; set; }


    public ICollection<Office> Offices { get; set; }

    public ICollection<WorkItem> CreatedWorkItems { get; set; }

    public ICollection<WorkItem> WorkItems { get; set; }

    public ICollection<SurveyResult> SurveyResults { get; set; }
  }

  /// <summary>
  /// Type of employee
  /// </summary>
  public enum EmployeeType
  {
    /// <summary>
    /// Director. It is an owner product
    /// </summary>
    Director,
    Manager,
    Employee,
    TechnicalSupport
  }
}
