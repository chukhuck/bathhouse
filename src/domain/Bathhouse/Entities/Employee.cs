using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.Entities
{
#nullable enable
  public class Employee : Entity
  {
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string LastName { get; set; } = "DefaultLastName";
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string MiddleName { get; set; } = String.Empty;
    [MaxLength(25, ErrorMessage = "Maximum field length exceeded. Max lenght of field is 25 symbols.")]
    public string FirstName { get; set; } = String.Empty;
    [NotMapped]
    public string FullName => LastName + " " + FirstName + " " + MiddleName;
    [NotMapped]
    public string ShortName => LastName + " " + 
                              (string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName.FirstOrDefault()) + "." + 
                              (string.IsNullOrEmpty(MiddleName) ? string.Empty : MiddleName.FirstOrDefault()) + ".";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? DoB { get; set; }
    public EmployeeType Type { get; set; }


    public ICollection<Office> Offices { get; set; } = null!;

    public ICollection<WorkItem> CreatedWorkItems { get; set; } = null!;

    public ICollection<WorkItem> WorkItems { get; set; } = null!;

    public ICollection<SurveyResult> SurveyResults { get; set; } = null!;

    public ICollection<Survey> Surveys { get; set; } = null!;

    public IEnumerable<Office> GetOffices()
    {
      return Offices;
    }

    public void DeleteOffice(Guid officeId)
    {
      if (Offices.FirstOrDefault(e => e.Id == officeId) is Office removingOffice)
        Offices.Remove(removingOffice);
    }

    public void AddOffice(Office addingOffice)
    {
      if (addingOffice == null)
        throw new ArgumentNullException("The adding office is null.");

      Offices.Add(addingOffice);
    }

    public IEnumerable<WorkItem> GetMyWorkItems()
    {
      return WorkItems;
    }

    public IEnumerable<WorkItem> GetCreatedWorkItems()
    {
      return CreatedWorkItems;
    }

    public IEnumerable<Survey> GetSurveys()
    {
      return Surveys;
    }
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
