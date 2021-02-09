using chukhuck.Helpers.Patterns;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.Entities
{
  public class Employee : IdentityUser<Guid>, IEntity<Guid>
  {
    public string LastName { get; set; } = "DefaultLastName";
    public string MiddleName { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string FullName => LastName + " " + FirstName + " " + MiddleName;
    public string ShortName => LastName + " " + 
                              (string.IsNullOrEmpty(FirstName) ? string.Empty : FirstName.FirstOrDefault()) + "." + 
                              (string.IsNullOrEmpty(MiddleName) ? string.Empty : MiddleName.FirstOrDefault()) + ".";
    public DateTime? DoB { get; set; }


    public virtual ICollection<Office> Offices { get; set; } = null!;

    public virtual ICollection<WorkItem> CreatedWorkItems { get; set; } = null!;

    public virtual ICollection<WorkItem> WorkItems { get; set; } = null!;

    public virtual ICollection<SurveyResult> SurveyResults { get; set; } = null!;

    public virtual ICollection<Survey> Surveys { get; set; } = null!;

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
  }
}
