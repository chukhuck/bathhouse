using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Entities
{
#nullable enable
  public class Employee : IdentityUser<Guid>
  {
    public string LastName { get; set; } = "DefaultLastName";
    public string MiddleName { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string FullName => LastName + " " + FirstName + " " + MiddleName;
    public string ShortName => LastName + " " + 
                              (string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName.FirstOrDefault()) + "." + 
                              (string.IsNullOrEmpty(MiddleName) ? string.Empty : MiddleName.FirstOrDefault()) + ".";
    public DateTime? DoB { get; set; }


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
        throw new ArgumentNullException(paramName: nameof(addingOffice));

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
}
