using System.Collections.Generic;

namespace Bathhouse.Contracts
{
  public partial class Constants
  {
    public const string OfficeModifyPolicy = "Office.Modify";
    public const string EmployeeAddOrDeletePolicy = "Employee.AddOrDelete";

    public static IEnumerable<string> GetPoliciesName()
    {
      yield return OfficeModifyPolicy;
      yield return EmployeeAddOrDeletePolicy;
    }
  }
}
